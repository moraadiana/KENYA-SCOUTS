using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.ViewModels
{
    public class AdvertisedJobs
    {
        public int Counter { get; set; }
        public string JobRefNo { get; set; }
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public decimal VacantPositions { get; set; }
        public decimal RequiredPositions { get; set; }
    }
}