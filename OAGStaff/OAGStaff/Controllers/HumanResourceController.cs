using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace OAGStaff.Controllers
{
    public class HumanResourceController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult LeaveListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                string username = Session["username"].ToString();
                var leaveListing = HRHelper.GetLeaveApplications(username);
                resource.LeaveApplications = leaveListing;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(resource);
        }

        public ActionResult LeaveApplication()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            string username = Session["username"].ToString();
            HumanResource resource = new HumanResource();
            var leaveTypes = HRHelper.GetLeaveTypes(username);
            var relievers = Helper.GetAllEmployees();
            var resCenters = Helper.GetResponsibilityCenters();
            var departmentDetails = Helper.GetEmployeeDepartmentDetails(username);
            resource.LeaveTypes = leaveTypes;
            resource.Relievers = relievers;
            resource.ResponsibilityCenters = resCenters;
            resource.Department = departmentDetails.Department;
            resource.Directorate = departmentDetails.Directorate;
            resource.SubDirectorate = departmentDetails.SubDirectorate;
            resource.UnitCode = departmentDetails.UnitCode;
            return View(resource);
        }

        [HttpPost]
        public ActionResult LeaveApplication(HumanResource leave)
        {
            try
            {
                string username = Session["username"].ToString();
                string reliever = leave.Reliever;
                string leaveType = leave.LeaveType;
                decimal appliedDays = Convert.ToDecimal(leave.AppliedDays);
                DateTime startDate = Convert.ToDateTime(leave.StartDate);
                DateTime endDate = Convert.ToDateTime(leave.EndDate);
                DateTime returnDate = Convert.ToDateTime(leave.ReturnDate);
                string purpose = leave.Purpose;
                string resCenter = leave.ResponsibilityCenter;
                string leaveBalance = leave.LeaveBalance;
                string comments = leave.Comments;

                if (webportals.HasPendingLeaveApplication(username))
                {
                    TempData["Error"] = "You have a pending leave application. Please wait or cancel it to apply for a new one";
                    return RedirectToAction("leaveapplication");
                }

                string response = webportals.HRMLeaveApplication(username, reliever, leaveType, appliedDays, startDate, endDate, returnDate, purpose, resCenter, comments);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string leaveNo = responseArr[1];
                        string approval = webportals.OnsendLeaveRequisitionForApproval(leaveNo);
                        if (approval == "SUCCESS")
                        {
                            TempData["Success"] = $"Leave application with number {leaveNo} has been sent for approval successfully";
                            return RedirectToAction("leavelisting");
                        }
                        else
                        {
                            TempData["Error"] = approval;
                            return RedirectToAction("leaveapplication");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "An error occured while creating leave application. Please try again later";
                        return RedirectToAction("leaveapplication");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("leaveapplication");
            }
            return View();
        }

        public ActionResult CancelLeave(string leaveNo)
        {
            try
            {
                webportals.OnCancelLeaveApplication(leaveNo);
                TempData["Success"] = $"Leave number {leaveNo} has been cancelled successfully";
                return RedirectToAction("leavelisting");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("leavelisting");
            }
        }

        public ActionResult LeaveStatement()
        {
            try
            {
                string pathDownloads = Server.MapPath("~/Downloads/");
                if (!Directory.Exists(pathDownloads))
                {
                    Directory.CreateDirectory(pathDownloads);
                }
                string username = Session["username"].ToString();
                string filename = username.Replace(@"/", "");
                string pdfFile = String.Format("LeaveStatement-{0}.pdf", filename);
                webportals.GenerateStaffLeaveStatement(username, pdfFile);
                ViewBag.filepath = Url.Content($"~/Downloads/{pdfFile}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        public ActionResult Payslip()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                var periodYears = HRHelper.GetPeriodYears();
                resource.PeriodYears = periodYears;
                string username = Session["username"].ToString();
                string filename = username.Replace("/", "");
                string pdfUrl = String.Format(@"Payslip-{0}.pdf", filename);
                ViewBag.filepath = Url.Content($"~/Downloads/{pdfUrl}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(resource);
        }

        [HttpPost]
        public ActionResult Payslip(HumanResource resource)
        {
            try
            {
                string username = Session["username"].ToString();
                string filename = username.Replace("/", "");
                string periodYear = resource.PeriodYear;
                string periondMonth = resource.PeriodMonth;
                string month = Convert.ToInt32(periondMonth) < 10 ? $"0{periondMonth}" : periondMonth;
                string periodDate = $"{month}/01/{periodYear}";
                var period = DateTime.ParseExact(periodDate, "M/dd/yyyy", CultureInfo.InvariantCulture);
                string pdfUrl = String.Format(@"Payslip-{0}.pdf", filename);
                //webportals.GeneratePayslipReport(username, period, pdfUrl);
                ViewBag.filepath = Url.Content($"~/Downloads/{pdfUrl}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("payslip");
            }
            return RedirectToAction("payslip");
        }

        public ActionResult Pnine()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            string username = Session["username"].ToString();
            //var p9Years = HRHelper.GetPnineYears(username);
            //resource.PeriodYears = p9Years;
            string filename = username.Replace("/", "");
            string pdfFile = String.Format(@"Pnine-{0}.pdf", filename);
            string filepath = Url.Content($"~/Downloads/{pdfFile}");
            ViewBag.filepath = filepath;
            return View(resource);
        }

        [HttpPost]
        public ActionResult Pnine(HumanResource resource)
        {
            try
            {
                int periodYear = Convert.ToInt32(resource.PeriodYear);
                string username = Session["username"].ToString();
                string filename = username.Replace("/", "");
                string pdfFile = String.Format(@"Pnine-{0}.pdf", filename);
                webportals.GenerateP9Report(username, periodYear, pdfFile);
                string filepath = Url.Content($"~/Downloads/{pdfFile}");
                ViewBag.filepath = filepath;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("pnine");
        }

        public ActionResult BtoListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                string username = Session["username"].ToString();
                var leaveBtos = HRHelper.GetStaffBtos(username);
                resource.StaffBtos = leaveBtos;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("btolisting", "humanresource");
            }
            return View(resource);
        }

        public ActionResult BtoHeader()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                string username = Session["username"].ToString();
                string staffName = Session["staffName"].ToString();
                var staffDetails = Helper.GetEmployeeDepartmentDetails(username);
                var postedLeaves = HRHelper.GetPostedLeaves(username);
                resource.Directorate = staffDetails.Directorate;
                resource.Department = staffDetails.Department;
                resource.StaffName = staffName;
                resource.StaffNo = username;
                resource.PostedLeaves = postedLeaves;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(resource);
        }

        [HttpPost]
        public ActionResult BtoHeader(HumanResource resource)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            try
            {
                string username = Session["username"].ToString();
                string leaveNo = resource.LeaveNo;

                string response = webportals.CreateLeaveBackToOffice(username, leaveNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string btoNumber = responseArr[1];
                        string approval = webportals.OnsendLeaveBtoForApproval(btoNumber);
                        if (!string.IsNullOrEmpty(approval))
                        {
                            if (approval == "SUCCESS")
                            {
                                TempData["Success"] = $"Leave back to office number {btoNumber} has been sent for approval successfully successfully.";
                                return RedirectToAction("btolisting", "humanresource");
                            }
                            else
                            {
                                TempData["Error"] = approval;
                                return RedirectToAction("btoheader", "humanresource");
                            }
                        }

                    }
                    else
                    {
                        TempData["Error"] = "An error occured while creating leave back to office. Please try again later!";
                        return RedirectToAction("btoheader", "humanresource");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("btoheader", "humanresource");
            }
            return RedirectToAction("btoheader", "humanresource");
        }

        public ActionResult CancelLeaveBto(string documentNo)
        {
            try
            {
                webportals.OnCancelLeaveBto(documentNo);
                TempData["Success"] = $"Leave back to office number {documentNo} has been cancelled successfully";
                return RedirectToAction("btolisting", "humanresource");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("btolisting", "humanresource");
            }
        }

        public ActionResult StudyLeaveListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                string username = Session["username"].ToString();
                var leaveListing = HRHelper.GetLeaveApplications(username);
                resource.LeaveApplications = leaveListing;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(resource);
        }

        public ActionResult StudyLeaveApplication()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            string username = Session["username"].ToString();
            HumanResource resource = new HumanResource();
            var leaveTypes = HRHelper.GetStudyLeaves();
            var relievers = Helper.GetAllEmployees();
            var resCenters = Helper.GetResponsibilityCenters();
            var departmentDetails = Helper.GetEmployeeDepartmentDetails(username);
            resource.LeaveTypes = leaveTypes;
            resource.Relievers = relievers;
            resource.ResponsibilityCenters = resCenters;
            resource.Department = departmentDetails.Department;
            resource.Directorate = departmentDetails.Directorate;
            resource.SubDirectorate = departmentDetails.SubDirectorate;
            resource.UnitCode = departmentDetails.UnitCode;
            return View(resource);
        }

        [HttpPost]
        public ActionResult StudyLeaveApplication(HumanResource leave)
        {
            try
            {
                string username = Session["username"].ToString();
                string leaveType = leave.LeaveType;
                decimal appliedDays = Convert.ToDecimal(leave.AppliedDays);
                DateTime startDate = Convert.ToDateTime(leave.StartDate);
                DateTime endDate = Convert.ToDateTime(leave.EndDate);
                DateTime returnDate = Convert.ToDateTime(leave.ReturnDate);
                string purpose = leave.Purpose;
                string resCenter = leave.ResponsibilityCenter;
                string leaveBalance = leave.LeaveBalance;
                string course = leave.Course;
                string university = leave.UniversityOfStudy;
                string country = leave.CountryOfStudy;
                int scholarship = leave.Scholarship;
                //DateTime levyStartDate = leave.LevyStartDate;
                //DateTime levyEndDate = leave.LevyEndDate;

                if (webportals.HasPendingLeaveApplication(username))
                {
                    TempData["Error"] = "You have a pending leave application. Please wait or cancel it to apply for a new one";
                    return RedirectToAction("studyleaveapplication", "humanresource");
                }

                string response = webportals.HRMStudyLeaveApplication(username, leaveType, appliedDays, startDate, endDate, returnDate, purpose, resCenter, course, university, country, scholarship);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string leaveNo = responseArr[1];
                        if (leave.AttachmentFile != null)
                        {
                            string fileName = leave.AttachmentFile.FileName.Replace(" ", "");
                            string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                            if (fileExtension == "pdf" || fileExtension == "doc" || fileExtension == "docx")
                            {
                                string path = Server.MapPath("~/Uploads/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                string pathToUpload = Path.Combine(path, leaveNo.Replace("/", "") + fileName);
                                if (System.IO.File.Exists(pathToUpload))
                                {
                                    System.IO.File.Delete(pathToUpload);
                                }
                                leave.AttachmentFile.SaveAs(pathToUpload);
                                Stream fs = leave.AttachmentFile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((int)fs.Length);
                                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                                webportals.RegFileUpload(leaveNo, fileName, base64String, 52179093);
                            }
                            else
                            {
                                TempData["Error"] = "Please upload files with .pdf, .doc and .docx extensions only";
                                return RedirectToAction("studyleaveapplication", "humanresource");
                            }
                        }
                        string approval = webportals.OnsendLeaveRequisitionForApproval(leaveNo);
                        if (approval == "SUCCESS")
                        {
                            TempData["Success"] = $"Leave application with number {leaveNo} has been sent for approval successfully";
                            return RedirectToAction("studyleavelisting", "humanresource");
                        }
                        else
                        {
                            TempData["Error"] = approval;
                            return RedirectToAction("studyleaveapplication", "humanresource");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "An error occured while creating leave application. Please try again later";
                        return RedirectToAction("studyleaveapplication", "humanresource");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("studyleaveapplication", "humanresource");
            }
            return View();
        }

        public string HasAppliedForLeaveType(string leaveType)
        {
            string username = Session["username"].ToString();
            string message = webportals.HasAppliedForLeave(username, leaveType);
            return message;
        }

        public ActionResult DisciplinaryListing()
        {
            HumanResource resource = new HumanResource();
            try
            {
                string username = Session["username"].ToString();
                var list = new List<HumanResource>();
                string disciplinary = webportals.GetDisciplinaryList(username);
                if (!string.IsNullOrEmpty(disciplinary))
                {
                    string[] disciplinaryArr = disciplinary.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in disciplinaryArr)
                    {
                        string[] responseArr = str.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new HumanResource()
                        {
                            DocumentNo = responseArr[0],
                            DocumentDate = Convert.ToDateTime(responseArr[1]),
                            IncidentDate = Convert.ToDateTime(responseArr[2]),
                            Description = responseArr[3],
                            Status = responseArr[4],
                            StatusCls = Components.StatusClass(responseArr[4]),
                            ActionType = responseArr[5],
                            ActionName = responseArr[6],
                            AccussedName = responseArr[7],
                        });
                    }
                }
                resource.DisciplinaryListing = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(resource);
        }

        public ActionResult DisciplinaryHeader()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                string username = Session["username"].ToString();
                string staffName = Session["staffName"].ToString();
                var incidentTypes = HRHelper.GetIncidentTypes();
                var employees = Helper.GetAllEmployees();
                resource.StaffNo = username;
                resource.StaffName = staffName;
                resource.IncidentTypes = incidentTypes;
                resource.Relievers = employees;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(resource);
        }

        [HttpPost]
        public ActionResult DisciplinaryHeader(HumanResource resource)
        {
            try
            {
                string username = Session["username"].ToString();
                string incidentType = resource.IncidentType;
                DateTime incidentDate = resource.IncidentDate;
                string accussedEmployee = resource.AccusedEmployee;
                string description = resource.Description;
                webportals.CreateDisciplinaryHeader(username, incidentType, incidentDate, accussedEmployee, description);
                TempData["Success"] = "Incident has been submitted successfully!";
                return RedirectToAction("disciplinarylisting", "humanresource");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("disciplinaryheader", "humanresource");
            }
        }

        public ActionResult LeaveRecallListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                string username = Session["username"].ToString();
                string recalls = webportals.GetLeaveRecalls(username);
                var list = new List<HumanResource>();
                if (!string.IsNullOrEmpty(recalls))
                {
                    string[] recallsArr = recalls.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string recall in recallsArr)
                    {
                        string[] responseArr = recall.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new HumanResource()
                        {
                            DocumentNo = responseArr[0],
                            Date = Convert.ToDateTime(responseArr[1]),
                            LeaveNo = responseArr[2],
                            StaffNo = responseArr[3],
                            StaffName = responseArr[4],
                            AppliedDays = responseArr[5],
                            UtilizedDays = responseArr[6],
                            DaysRecalled = responseArr[7],
                            LeaveType = responseArr[8],
                            Status = responseArr[9],
                            StatusCls = Components.StatusClass(responseArr[9])
                        });
                    }
                }
                resource.LeaveRecalls = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(resource);
        }

        public ActionResult LeaveRecall()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            HumanResource resource = new HumanResource();
            try
            {
                var leavesToRecall = HRHelper.GetLeavesToRecall();
                resource.LeaveRecalls = leavesToRecall;
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(resource);
        }

        [HttpPost]
        public ActionResult LeaveRecall(HumanResource resource)
        {
            try
            {
                string leaveNo = resource.LeaveNo;
                string reason = resource.Description;
                string username = Session["username"].ToString();
                DateTime date = resource.Date;
                string recallNo = webportals.CreateLeaveRecall(username, leaveNo, reason,date);
                webportals.OnSendLeaveRecallForApproval(recallNo);
                TempData["Success"] = "Leave recall has been initiated successfully!";
                return RedirectToAction("leaverecalllisting", "humanresource");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("leaverecall", "humanresource");
            }
        }

        public ActionResult CancelLeaveRecall(string recallNo)
        {
            try
            {
                webportals.OnCancelLeaveRecallApprovalRequest(recallNo);
                TempData["Success"] = "Leave recall has been cancelled successfully!";
                return RedirectToAction("leaverecalllisting", "humanresource");
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("leaverecalllisting", "humanresource");
            }
        }
    }
}