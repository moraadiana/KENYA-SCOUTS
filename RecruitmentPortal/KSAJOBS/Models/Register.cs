using KSAJOBS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.Models
{
    public class Register
    {
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string PostalAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public string ApplicantType { get; set; }
        public string EmailAddress { get; set; }
        public string Disability { get; set; }
        public string DisabilityStatus { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Citizenship { get; set; }
        public List<Citizenship> CitizenshipList { get; set; }
    }
}