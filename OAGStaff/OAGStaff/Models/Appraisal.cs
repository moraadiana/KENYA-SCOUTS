using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff.Models
{
    public class Appraisal
    {
        public string AppraisalNo { get; set; }
        public string Date { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
        public string Period { get; set; }
        public string Status { get; set; }
        public string StatusCls { get; set; }
        public string Directorate { get; set; }
        public string Department { get; set; }
        public string Supervisor { get; set; }
        public string SubCode { get; set; }
        public string SubActivity { get; set; }
        public string PerformanceCriteria { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Target { get; set; }
        public string Description { get; set; }
        public string SelfAssessment { get; set; }
        public string SupervisorAssessment { get; set; }
        public string AgreedRating { get; set; }
        public bool MidYearReview { get; set; }
        public bool EndYearReview { get; set; }
        public DateTime MidYearStartDate { get; set; }
        public DateTime MidYearEndDate { get; set; }
        public DateTime EndYearStartDate { get; set; }
        public DateTime EndYearEndDate { get; set; }
        public int Rating { get; set; }
        public string SelectedCategories { get; set; }
        public string SelfAssessmentCategories { get; set; }
        public List<Appraisal> Appraisals { get; set; }
        public List<App> Employees { get; set; }
        public List<Appraisal> AppraisalsSubLines { get; set; }
        public List<Appraisal> AppraisalsValues { get; set; }
        public List<Appraisal> MidYearDates { get; set; }
        public List<Appraisal> AssessmentScores { get; set; }
    }
}