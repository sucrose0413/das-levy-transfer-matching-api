﻿using SFA.DAS.LevyTransferMatching.Models.Enums;
using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Abstractions;
using SFA.DAS.LevyTransferMatching.Data.ValueObjects;
using SFA.DAS.LevyTransferMatching.Domain.Events;

namespace SFA.DAS.LevyTransferMatching.Data.Models
{
    public class Pledge : AggregateRoot<int>
    {
        protected Pledge() {}

        public Pledge(EmployerAccount employerAccount, CreatePledgeProperties properties, UserInfo userInfo)
        {
            EmployerAccount = employerAccount;
            Amount = properties.Amount;
            RemainingAmount = properties.Amount;
            IsNamePublic = properties.IsNamePublic;
            Levels = properties.Levels;
            JobRoles = properties.JobRoles;
            Sectors = properties.Sectors;
            CreatedOn = DateTime.UtcNow;
            _locations = properties.Locations;

            StartTrackingSession(UserAction.CreatePledge, employerAccount.Id, userInfo);
            ChangeTrackingSession.TrackInsert(this);
            foreach (var location in _locations)
            {
                ChangeTrackingSession.TrackInsert(location);
            }
        }

        public EmployerAccount EmployerAccount { get; private set; }

        public int Amount { get; private set; }

        public int RemainingAmount { get; private set; }

        public bool IsNamePublic { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public JobRole JobRoles { get; private set; }
        
        public Level Levels { get; private set; }

        public Sector Sectors { get; private set; }

        private readonly List<PledgeLocation> _locations;
        public IReadOnlyCollection<PledgeLocation> Locations => _locations;

        public byte[] RowVersion { get; private set; }

        public Application CreateApplication(EmployerAccount account, CreateApplicationProperties properties, UserInfo userInfo)
        {
            return new Application(this, account, properties, userInfo);
        }
    }
}