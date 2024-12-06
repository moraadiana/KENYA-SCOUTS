using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGJobs.Models
{
    public class Applicant
    {
        public int Counter { get; set; }
        public string ApplicationNo { get; set; }
        public string IdNumber { get; set; }
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobRefNo { get; set; }
        public string QualificationType { get; set; }
        public string QualificationCode { get; set; }
        public string QualificationDescription { get; set; }
        public string Code { get; set; }
        public string Institution { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Course { get; set; }
        public string Grade { get; set; }
        public int Type { get; set; }
        public string RefereeName { get; set; }
        public string RefereeDesignation { get; set; }
        public string RefereeInstitution { get; set; }
        public string RefereeEmail { get; set; }
        public string RefereePhone { get; set; }
        public string RefereeAddress { get; set; }
        public string RefereePostCode { get; set; }
        public string RefereeCity { get; set; }
        public string RefereePeriod { get; set; }
        public string Skill { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Hobby { get; set; }
        public string ProffessionalBody { get; set; }
        public string MembershipNo { get; set; }
        public string MembershipType { get; set; }
        public DateTime DateOfRenewal { get; set; }
        public string SystemId { get; set; }
        public string Answer { get; set; }
        public string SelectedCategories { get; set; }
        public string Award { get; set; }
        public string Specialization { get; set; }
        public string NokName { get; set; }
        public string NokAddress { get; set; }
        public string NokTel { get; set; }
        public string NokRelationship { get; set; }
        public string Duration { get; set; }
        public int Year { get; set; }
        public string Designation { get; set; }
        public string JobGrade { get; set; }
        public decimal Salary { get; set; }
        public string AreaOfStudy { get; set; }
        public string AreaOfSpecialization { get; set; }
        public HttpPostedFileBase AttachmentFile { get; set; }
        public List<Applicant> QualificationTypes { get; set; }
        public List<Applicant> MinimumRequirements { get; set; }
        public List<Applicant> SubmittedMinimumRequirements { get; set; }
        public List<Applicant> ApplicantQualifications { get; set; }
        public List<Applicant> ApplicantProfessionalQualifications { get; set; }
        public List<Applicant> ProffessionalBodies { get; set; }
        public List<Applicant> ProffessionalQualifications { get; set; }
        public List<Applicant> ApplicantNok { get; set; }
        public List<Applicant> ApplicantReferees { get; set; }
        public List<Applicant> ApplicantAttachments { get; set; }
        public List<Applicant> Relationships { get; set; }
        public List<Applicant> ApplicantTrainings { get; set; }
        public List<Applicant> ApplicantEmploymentDetails { get; set; }
        public List<Applicant> AreasOfStudy { get; set; }
    }
}