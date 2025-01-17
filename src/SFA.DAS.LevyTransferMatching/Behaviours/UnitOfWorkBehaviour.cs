﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.UnitOfWork.Managers;

namespace SFA.DAS.LevyTransferMatching.Behaviours
{
    public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkBehaviour(ILogger<UnitOfWorkBehaviour<TRequest, TResponse>> logger, IUnitOfWorkManager unitOfWorkManager)
        {
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogTrace("Invoked UnitOfWorkBehaviour");

            TResponse result;

            try
            {
                await _unitOfWorkManager.BeginAsync();

                result = await next();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception handled by UnitOfWorkBehaviour", ex);

                await _unitOfWorkManager.EndAsync(ex);
                throw;
            }

            await _unitOfWorkManager.EndAsync();

            return result;
        }
    }
}
