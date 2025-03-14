using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff.Models
{
    public class Recruitment
    {
        public string RequisitionNo { get; set; }
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ApplicantNo { get; set; }
        public string ApplicantName { get; set; }
        public int Shortlisted { get; set; }
        public string Remarks { get; set; }
        public string SelectedCategories { get; set; }
        public string Comments { get; set; }
        public List<Recruitment> Requisitions { get; set; }
        public List<Recruitment> JobApplicants { get; set; }
    }
}