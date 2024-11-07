using KSAJOBS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.Models
{
    public class Referee
    {
        public string RefereeName { get; set; }
        public string RefereeDesignation { get; set; }
        public string RefereeInstitution { get; set; }
        public string RefereeEmail { get; set; }
        public string RefereePhone { get; set; }
        public string RefereeAddress { get; set; }
        public List<ApplicantReferee> ApplicantReferees { get; set; }
    }
}