using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.ViewModels
{
    public class MyApplication
    {
        public int Counter { get; set; }
        public string JobApplicationNo { get; set; }
        public string JobId { get; set; }
        public string JobRefNo { get; set; }
        public string JobTitle { get; set; }
        public DateTime DateApplied { get; set; }
        public string Status { get; set; }
        public string InterviewStatus { get; set; }
    }
}