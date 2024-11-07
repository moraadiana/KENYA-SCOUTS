using KSAJOBS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.Models
{
    public class Qualification
    {
        public string QualificationType { get; set; }
        public string QualificationCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Institution { get; set; }
        public string Type { get; set; }
        public List<ApplicantQualification> ApplicantQualifications { get; set; }
    }
}