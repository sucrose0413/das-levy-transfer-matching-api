﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NServiceBus;
using SFA.DAS.LevyTransferMatching.Messages.Commands;

namespace SFA.DAS.LevyTransferMatching.Api.HealthChecks
{
    public class NServiceBusHealthCheck : IHealthCheck
    {
        public TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(500);
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

        private readonly IMessageSession _messageSession;
        private readonly IDistributedCache _distributedCache;

        public NServiceBusHealthCheck(IMessageSession messageSession, IDistributedCache distributedCache)
        {
            _messageSession = messageSession;
            _distributedCache = distributedCache;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var messageId = Guid.NewGuid();
            var data = new Dictionary<string, object> { ["MessageId"] = messageId };
            var sendOptions = new SendOptions();
            var stopwatch = Stopwatch.StartNew();

            sendOptions.SetMessageId(messageId.ToString());

            await _messageSession.Send(new RunHealthCheckCommand() { MessageId = messageId.ToString() }, sendOptions);

            while (!cancellationToken.IsCancellationRequested)
            {
                if (await _distributedCache.GetStringAsync(messageId.ToString(), cancellationToken) != null)
                {
                    return HealthCheckResult.Healthy(null, data);
                }

                if (stopwatch.Elapsed > Timeout)
                {
                    return HealthCheckResult.Degraded($"Potential issue with receiving endpoint (failed to handle {nameof(RunHealthCheckCommand)} in sufficient time)", null, data);
                }

                await Task.Delay(Interval, cancellationToken);
            }
            
            throw new OperationCanceledException(cancellationToken);
        }
    }
}
