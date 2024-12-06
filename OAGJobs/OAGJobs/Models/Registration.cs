using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGJobs.Models
{
    public class Registration
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string MobileNo { get; set; }
        public int Gender { get; set; }
        public int MaritalStatus { get; set; }
        public string PostalCode { get; set; }
        public string PostalAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public int ApplicantType { get; set; }
        public int Disability { get; set; }
        public string DisabilityStatus { get; set; }
        public string ConfirmPassword { get; set; }
        public string Nationality { get; set; }
        public string County { get; set; }
        public string SubCounty { get; set; }
        public string Constituency { get; set; }
        public string Tribe { get; set; }
        public string PhysicalAddress { get; set; }
        public string KraPin { get; set; }
        public string Town { get; set; }
        public string TelephoneNo { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonTel { get; set; }
        public string NatureOfDisability { get; set; }
        public string RegistrationNo { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int EthnicOrigin { get; set; }
        public List<Registration> Nationalities { get; set; }
        public List<Registration> Counties { get; set; }
        public List<Registration> Tribes { get; set; }
    }
}