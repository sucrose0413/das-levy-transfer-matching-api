﻿using SFA.DAS.LevyTransferMatching.Models.Enums;
using System;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Pledge
    {
        public int Id { get; set; }

        public long AccountId { get; set; }

        public int Amount { get; set; }

        public bool IsNamePublic { get; set; }

        public string DasAccountName { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<JobRole> JobRoles { get; set; }

        public IEnumerable<Level> Levels { get; set; }

        public IEnumerable<Sector> Sectors { get; set; }
        public int RemainingAmount { get; set; }
    }
}