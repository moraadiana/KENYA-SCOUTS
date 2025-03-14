using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff.Models
{
    public class Approval
    {
        public int Counter { get; set; }
        public int EntryNo { get; set; }
        public int SequenceNo { get; set; }
        public DateTime DateSentForApproval { get; set; }
        public string SenderId { get; set; }
        public string ApproverId { get; set; }
        public string Status { get; set; }
        public string StatusCls { get; set; }
        public string Comments { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string RecordId { get; set; }
        public string WorkflowInstanceId { get; set; }
        public string TableId { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string JobGrade { get; set; }
        public string Reason { get; set; }
        public string VacantPositions { get; set; }
        public string RequiredPositions { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string RelieverName { get; set; }
        public string LeaveType { get; set; }
        public string AppliedDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<Approval> Approvals { get; set; }
        public List<Approval> LeaveApprovals { get; set; }
    }
}