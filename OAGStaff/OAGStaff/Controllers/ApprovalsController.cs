using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGStaff.Controllers
{
    public class ApprovalsController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult ApprovalTracking(string documentNo)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Approval approval = new Approval();
            try
            {
                var approvalEntries = ApprovalsHelper.GetApprovals(documentNo);
                approval.Approvals = approvalEntries;
                ViewBag.DocumentNo = documentNo;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(approval);
        }

        public ActionResult LeaveApprovals()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Approval approval = new Approval();
            try
            {
                string username = Session["username"].ToString();
                string userId = webportals.GetEmployeeUserId(username);
                var list = new List<Approval>();
                string leaveApprovals = webportals.GetLeaveApprovalRequests(userId);
                if (!string.IsNullOrEmpty(leaveApprovals))
                {
                    string[] leaveApprovalsArr = leaveApprovals.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in leaveApprovalsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Approval()
                        {
                            DocumentType = responseArr[0],
                            DocumentNo = responseArr[1],
                            SenderId = responseArr[2],
                            RecordId = responseArr[3],
                            WorkflowInstanceId = responseArr[4],
                            TableId = responseArr[5],
                        });
                    }
                }
                approval.LeaveApprovals = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(approval);
        }

        public ActionResult EmployeeRequisitions()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Approval approval = new Approval();
            try
            {
                string username = Session["username"].ToString();
                string userId = webportals.GetEmployeeUserId(username);
                var list = new List<Approval>();
                string leaveApprovals = webportals.GetEmployeeRequisitionsApprovalRequests(userId);
                if (!string.IsNullOrEmpty(leaveApprovals))
                {
                    string[] leaveApprovalsArr = leaveApprovals.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in leaveApprovalsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Approval()
                        {
                            DocumentType = responseArr[0],
                            DocumentNo = responseArr[1],
                            SenderId = responseArr[2],
                            RecordId = responseArr[3],
                            WorkflowInstanceId = responseArr[4],
                            TableId = responseArr[5],
                        });
                    }
                }
                approval.LeaveApprovals = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(approval);
        }

        public ActionResult EmployeeRequisitionDetails(string documentNo, string workflowId)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Approval approval = new Approval();
            try
            {
                string response = webportals.GetEmployeeRequisitionDetails(documentNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    approval.Date = Convert.ToDateTime(responseArr[0]);
                    approval.Code = responseArr[1];
                    approval.Description = responseArr[2];
                    approval.JobGrade = responseArr[3];
                    approval.Reason = responseArr[4];
                    approval.VacantPositions = responseArr[5];
                    approval.RequiredPositions = responseArr[6];
                }
                approval.DocumentNo = documentNo;
                approval.WorkflowInstanceId = workflowId;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(approval);
        }

        public ActionResult LeaveRequisitionDetails(string documentNo, string workflowId)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Approval approval = new Approval();
            try
            {
                string response = webportals.GetLeaveDetails(documentNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    approval.EmployeeName = responseArr[1];
                    approval.LeaveType = responseArr[2];
                    approval.AppliedDays = responseArr[3];
                    approval.StartDate = Convert.ToDateTime(responseArr[4]);
                    approval.EndDate = Convert.ToDateTime(responseArr[5]);
                    approval.ReturnDate = Convert.ToDateTime(responseArr[6]);
                    approval.Description = responseArr[7];
                }
                approval.DocumentNo = documentNo;
                approval.WorkflowInstanceId = workflowId;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(approval);
        }

        public ActionResult CancelApprovalRequest(Approval approval)
        {
            try
            {
                string documentNo = approval.DocumentNo;
                string workflowId = approval.WorkflowInstanceId;
                string comment = approval.Comments;
                webportals.RejectApprovalRequests(documentNo, workflowId, comment);
                TempData["Success"] = $"Document number {documentNo} has been cancelled successfully!";
                return RedirectToAction("index", "dashboard");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
        }

        public ActionResult ApproveApprovalRequest(string documentNo, string workflowId)
        {
            try
            {
                webportals.ApproveApprovalRequests(documentNo,workflowId);
                TempData["Success"] = $"Document number {documentNo} has been approved successfully";
                return RedirectToAction("index", "dashboard");
            }
            catch( Exception ex )
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
        }

        public ActionResult DelegateApprovalRequest(string documentNo, string workflowId)
        {
            try
            {
                webportals.DelegateApprovalRequests(documentNo, workflowId);
                TempData["Success"] = $"Document number {documentNo} has been approved successfully";
                return RedirectToAction("index", "dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
        }
    }
}