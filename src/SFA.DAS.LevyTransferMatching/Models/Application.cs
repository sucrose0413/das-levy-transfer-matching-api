﻿using System;
using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string DasAccountName { get; set; }
        public int PledgeId { get; set; }
        public string Details { get; set; }
        public string StandardId { get; set; }
        public int NumberOfApprentices { get; set; }
        public DateTime StartDate { get; set; }
        public int Amount { get; set; }
        public bool HasTrainingProvider { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
        public string Postcode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessWebsite { get; set; }
        public IEnumerable<string> EmailAddresses { get; set; }
        public DateTime CreateDate { get; set; }
    }
}