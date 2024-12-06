using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGJobs.Models
{
    public class Job
    {
        public int Counter { get; set; }
        public string RefNo { get; set; }
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public decimal RequiredPositions { get; set; }
        public string Description { get; set; }
        public string ApplicationNo { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public List<Job> AdvertisedJobs { get; set; }
        public List<Job> JobRequirements { get; set; }
        public List<Job> JobResponsibilities { get; set; }
        public List<Job> MyApplications { get; set; }
    }
}