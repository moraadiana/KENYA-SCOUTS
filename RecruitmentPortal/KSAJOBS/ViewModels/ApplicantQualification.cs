using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.ViewModels
{
    public class ApplicantQualification
    {
        public int Counter { get; set; }
        public string ApplitionNo { get; set; }
        public string QualificationType { get; set; }
        public string QualificationCode { get; set; }
        public string QualificationDescription { get; set; }
        public string Institution { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}