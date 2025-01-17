﻿using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledge
{
    public class GetPledgeQuery : IRequest<GetPledgeResult>
    {
        public int Id { get; set; }
    }
}