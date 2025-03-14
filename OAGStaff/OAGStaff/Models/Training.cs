using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff.Models
{
    public class Training
    {
        public string StaffNo { get; set; }
        public string DocumentNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Period { get; set; }
        public string CourseTitle { get; set; }
        public string Status { get; set; }
        public string StaffName { get; set; }
        public string Submitted { get; set; }
        public string Directorate { get; set; }
        public string Department { get; set; }
        public string SubDirectorate { get; set; }
        public string UnitCode { get; set; }
        public string HighestQualification { get; set; }
        public string Other { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string No { get; set; }
        public string Answer { get; set; }
        public string MonthYear { get; set; }
        public string TrainingInstitution { get; set; }
        public string Duration { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Course { get; set; }
        public int ApplicationType { get; set; }
        public int TrainingClassification { get; set; }
        public string TrainingNeed { get; set; }
        public string TrainingObjective { get; set; }
        public int TrainingType { get; set; }
        public string Trainer { get; set; }
        public int ModeOfTraining { get; set; }
        public string Venue { get; set; }
        public decimal BudgetAmount { get; set; }
        public string DsaVote { get; set; }
        public string TrainingVote { get; set; }
        public string TransportVote { get; set; }
        public string Vote { get; set; }
        public string VoteName { get; set; }
        public decimal CourseFee { get; set; }
        public decimal DsaAmount { get; set; }
        public decimal TransportCost { get; set; }
        public string ApplicationNo { get; set; }
        public string RelevantPoints { get; set; }
        public string Recommendations { get; set; }
        public string Comments { get; set; }
        public string CourseContent { get; set; }
        public string Gender { get; set; }
        public string JobTitle { get; set; }
        public HttpPostedFileBase AttachmentFile { get; set; }
        public List<Training> TrainingApplications { get; set; }
        public List<Training> TrainingNeeds { get; set; }
        public List<Training> AcademicQualifications { get; set; }
        public List<Training> ProffessionalQualifications { get; set; }
        public List<Training> TrainingNeedsProffessionalQualifications { get; set; }
        public List<Training> ProffessionalCourses { get; set; }
        public List<Training> TrainingGaps { get; set; }
        public List<Training> TrainingIntervensions { get; set; }
        public List<Training> TrainingPeriods { get; set; }
        public List<Training> TrainingCourses { get; set; }
        public List<Training> TrainingVotes { get; set; }
        public List<Training> TrainingLines { get; set; }
        public List<Training> TrainingBtos { get; set; }
        public List<Training> Trainings { get; set; }
        public List<App> TrainingEmployees { get; set; }
    }
}