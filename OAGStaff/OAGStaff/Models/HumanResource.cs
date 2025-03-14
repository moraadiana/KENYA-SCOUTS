using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff.Models
{
    public class HumanResource
    {
        public int Counter { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
        public string LeaveNo { get; set; }
        public string LeaveType { get; set; }
        public string LeaveBalance { get; set; }
        public string AppliedDays { get; set; }
        public string UtilizedDays { get; set; }
        public string DaysRecalled { get; set; }
        public string ApplicationDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ReturnDate { get; set; }
        public string Status { get; set; }
        public string StatusCls { get; set; }
        public string Reliever { get; set; }
        public string ResponsibilityCenter { get; set; }
        public string Department { get; set; }
        public string Directorate { get; set; }
        public string SubDirectorate { get; set; }
        public string UnitCode { get; set; }
        public string Purpose { get; set; }
        public string Comments { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PeriodYear { get; set; }
        public string PeriodName { get; set; }
        public string PeriodMonth { get; set; }
        public string DocumentNo { get; set; }
        public string Course { get; set; }
        public string PeriodOfStudy { get; set; }
        public string UniversityOfStudy { get; set; }
        public string CountryOfStudy { get; set; }
        public int Scholarship { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime IncidentDate { get; set; }
        public string IncidentType { get; set; }
        public string AccusedEmployee { get; set; }
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string AccussedName { get; set; }
        public DateTime Date { get; set; }

        //public DateTime LevyStartDate { get; set; }
        //public DateTime LevyEndDate { get; set; }
        public HttpPostedFileBase AttachmentFile { get; set; }
        public List<HumanResource> LeaveApplications { get; set; }
        public List<HumanResource> LeaveTypes { get; set; }
        public List<HumanResource> PeriodYears { get; set; }
        public List<HumanResource> StaffBtos { get; set; }
        public List<HumanResource> PostedLeaves { get; set; }
        public List<HumanResource> DisciplinaryListing { get; set; }
        public List<HumanResource> IncidentTypes { get; set; }
        public List<HumanResource> LeaveRecalls { get; set; }
        public List<App> Relievers { get; set; }
        public List<App> ResponsibilityCenters { get; set; }
    }
}