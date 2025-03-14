using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGStaff.Controllers
{
    public class TrainingController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult TrainingListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            try
            {
                string username = Session["username"].ToString();
                var myTrainings = TrainingHelper.GetMyTrainingApplications(username);
                training.TrainingApplications = myTrainings;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(training);
        }

        public ActionResult TrainingHeader()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            string username = Session["username"].ToString();
            string staffName = Session["staffName"].ToString();
            string response = webportals.GetStaffDetails(username);
            if (!string.IsNullOrEmpty(response))
            {
                string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                string jobId = responseArr[1];
                string jobTitle = responseArr[2];
                string gender = responseArr[3];
                string idNumber = responseArr[4];
                string emailAddress = responseArr[5];
                string phoneNumber = responseArr[6];
                string postalAddress = responseArr[7];
                
                training.Gender = gender;
                training.JobTitle= jobTitle;
            }
            var staffDetails = Helper.GetEmployeeDepartmentDetails(username);
            var trainingPeriods = TrainingHelper.GetTrainingPeriods();
            var trainingCourses = TrainingHelper.GetTrainingCourses();
            var trainingVotes = TrainingHelper.GetTrainingVotes();
            training.Department = staffDetails.Department;
            training.Directorate = staffDetails.Directorate;
            training.SubDirectorate = staffDetails.SubDirectorate;
            training.UnitCode = staffDetails.UnitCode;
            training.StaffNo = username;
            training.StaffName = staffName;
            training.TrainingPeriods = trainingPeriods;
            training.TrainingCourses = trainingCourses;
            training.TrainingVotes = trainingVotes;
            return View(training);
        }

        [HttpPost]
        public ActionResult TrainingHeader(Training training)
        {
            try
            {
                string username = Session["username"].ToString();
                string period = training.Period;
                DateTime startDate = training.StartDate;
                DateTime endDate = training.EndDate;
                string course = training.Course;
                int applicationType = training.ApplicationType;
                int trainingClassification = training.TrainingClassification;
                string trainingNeed = training.TrainingNeed;
                string trainingObjective = training.TrainingObjective;
                int type = training.TrainingType;
                string trainer = training.Trainer;
                int modeOfTraining = training.ModeOfTraining;
                string venue = training.Venue;
                string dsaVote = training.DsaVote;
                string trainingVote = training.TrainingVote;
                string transportVote = training.TransportVote;

                string response = webportals.CreateTrainingApplicationHeader(username, period, startDate, endDate, course, applicationType, trainingClassification, trainingNeed, trainingObjective, type, modeOfTraining, venue, dsaVote, trainingVote, transportVote, trainer);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string trainingNo = responseArr[1];
                        TempData["Success"] = $"Training application number {trainingNo} has been created successfully";
                        return RedirectToAction("traininglines", new { documentNo = trainingNo, status = "New" });
                    }
                    else
                    {
                        TempData["Error"] = "";
                        return RedirectToAction("trainingheader");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingheader");
            }
            return View();
        }

        public ActionResult TrainingLines(string documentNo, string status)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            string username = Session["username"].ToString();
            string staffName = Session["staffName"].ToString();
            var staffDetails = Helper.GetEmployeeDepartmentDetails(username);
            var trainingLines = TrainingHelper.GetTrainingLines(documentNo);
            var employees = Helper.GetAllEmployees();
            training.Department = staffDetails.Department;
            training.Directorate = staffDetails.Directorate;
            training.SubDirectorate = staffDetails.SubDirectorate;
            training.UnitCode = staffDetails.UnitCode;
            training.StaffName = staffName;
            training.DocumentNo = documentNo;
            training.TrainingLines = trainingLines;
            training.TrainingEmployees = employees;
            training.Status = status;
            Session["DocumentNo"] = documentNo;
            Session["Status"] = status;
            return View(training);
        }

        [HttpPost]
        public ActionResult TrainingLines(Training training)
        {
            string documentNo = Session["DocumentNo"].ToString();
            string status = Session["Status"].ToString();
            try
            {
                string staffNo = training.StaffNo;
                decimal courseFee = training.CourseFee;
                decimal dsaAmount = training.DsaAmount;
                decimal transportCost = training.TransportCost;
                if (webportals.InsertTrainingApplicationLines(documentNo, staffNo, courseFee, dsaAmount, transportCost))
                {
                    TempData["Success"] = "Line has been added successfully";
                    return RedirectToAction("traininglines", new { documentNo, status });
                }
                else
                {
                    TempData["Error"] = "An error occured while adding employees. Please try again later!";
                    return RedirectToAction("traininglines", new { documentNo, status });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("traininglines", new { documentNo, status });
            }
        }

        public ActionResult RemoveTrainingLines(string staffNo, string documentNo)
        {
            string status = Session["Status"].ToString();
            try
            {
                if (status == "New")
                {
                    if (webportals.RemoveTrainingApplicationNo(documentNo, staffNo))
                    {
                        TempData["Success"] = "Line deleted successfully";
                        return RedirectToAction("traininglines", new { documentNo, status });
                    }
                    else
                    {
                        TempData["Error"] = "An error occured while deleting line. Please try again later!";
                        return RedirectToAction("traininglines", new { documentNo, status });
                    }
                }
                else
                {
                    TempData["Error"] = "You can only edit open documents!";
                    return RedirectToAction("traininglines", new { documentNo, status });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("traininglines", new { documentNo, status });
            }
        }

        [HttpPost]
        public ActionResult TrainingApprovalRequest()
        {
            string documentNo = Session["DocumentNo"].ToString();
            string status = Session["Status"].ToString();
            try
            {
                var trainingLines = TrainingHelper.GetTrainingLines(documentNo);
                if (trainingLines.Count < 1)
                {
                    TempData["Error"] = "Please add lines before sending for approval";
                    return RedirectToAction("traininglines", new { documentNo, status });
                }
                string response = webportals.OnSendTrainingApplicationForApproval(documentNo);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["Success"] = "Document has been sent for approval successfully";
                        return RedirectToAction("traininglisting");
                    }
                    else
                    {
                        TempData["Error"] = response;
                        return RedirectToAction("traininglines", new { documentNo, status });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("traininglines", new { documentNo, status });
            }
            return View();
        }

        public ActionResult CancellTrainingApplication()
        {
            string documentNo = Session["DocumentNo"].ToString();
            string status = Session["Status"].ToString();
            try
            {
                webportals.OnCancelTrainingApplication(documentNo);
                TempData["Success"] = "document has been cancelled successfully";
                return RedirectToAction("traininglisting");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("traininglines", new { documentNo, status });
            }
        }

        public ActionResult TrainingNeedsListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            string username = Session["username"].ToString();
            var trainingNeeds = TrainingHelper.GetTrainingNeeds(username);
            training.TrainingNeeds = trainingNeeds;
            return View(training);
        }

        public ActionResult TrainingNeedsHeader()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            string username = Session["username"].ToString();
            string staffName = Session["staffName"].ToString();
            string response = webportals.GetStaffDetails(username);
            if (!string.IsNullOrEmpty(response))
            {
                string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                string jobId = responseArr[1];
                string jobTitle = responseArr[2];
                string gender = responseArr[3];
                string idNumber = responseArr[4];
                string emailAddress = responseArr[5];
                string phoneNumber = responseArr[6];
                string postalAddress = responseArr[7];

                training.Gender = gender;
                training.JobTitle = jobTitle;
            }
            var staffDetails = Helper.GetEmployeeDepartmentDetails(username);
            var academicQualifications = TrainingHelper.GetAcademicQualifications(username);
            training.AcademicQualifications = academicQualifications;
            training.Department = staffDetails.Department;
            training.Directorate = staffDetails.Directorate;
            training.StaffNo = username;
            training.StaffName = staffName;
            return View(training);
        }

        [HttpPost]
        public ActionResult TrainingNeedsHeader(Training training)
        {
            try
            {
                string username = Session["username"].ToString();
                string highestQualification = training.HighestQualification;
                string other = training.Other;

                string response = webportals.CreateTrainingNeedsAssessmentHeader(username, highestQualification, other);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string trainingNo = responseArr[1];
                        return RedirectToAction("trainingneedslines", new { documentNo = trainingNo, submitted = "No" });
                    }
                    else
                    {
                        TempData["Error"] = "An error occured while creating the training needs assessment header. Please try again later!";
                        return RedirectToAction("trainingneedsheader");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedsheader");
            }
            return View();
        }

        public ActionResult TrainingNeedsLines(string documentNo, string submitted)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            Session["DocumentNo"] = documentNo;
            Session["Submitted"] = submitted;
            training.DocumentNo = documentNo;
            var proffessionalQualifications = TrainingHelper.GetProffessionalQualifications();
            var trainingNeedsProffessionalQualifications = TrainingHelper.GetTrainingProffessionalQualifications(documentNo);
            var proffessionalCourses = TrainingHelper.GetProffessionalCourses(documentNo);
            var trainingGaps = TrainingHelper.GetTrainingGaps(documentNo);
            var trainingIntervensions = TrainingHelper.GetTrainingIntervensions(documentNo);
            training.ProffessionalQualifications = proffessionalQualifications;
            training.TrainingNeedsProffessionalQualifications = trainingNeedsProffessionalQualifications;
            training.ProffessionalCourses = proffessionalCourses;
            training.TrainingGaps = trainingGaps;
            training.TrainingIntervensions = trainingIntervensions;
            return View(training);
        }

        [HttpPost]
        public ActionResult ProffessionalQualifications(Training training)
        {
            string documentNo = Session["DocumentNo"].ToString();
            string submitted = Session["Submitted"].ToString();
            try
            {
                string code = training.Code;
                if (webportals.InsertTrainingNeedsProffessionalQualifications(documentNo, code))
                {
                    TempData["Success"] = "Line added successfully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while adding the line. Please try again later!";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        public ActionResult RemoveProffessionalQualifications(string documentNo, string no, string submitted)
        {
            try
            {
                if (submitted == "Yes")
                {
                    TempData["Error"] = "You cannot edit submitted documents";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                if (webportals.RemoveTrainingNeedsProffessionalQualifications(documentNo, no))
                {
                    TempData["Success"] = "Line deleted successfully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while deleting the line. Please try again later!";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        [HttpPost]
        public ActionResult ProffessionalCourses(Training training)
        {
            string documentNo = Session["DocumentNo"].ToString();
            string submitted = Session["Submitted"].ToString();
            try
            {
                string courseTitle = training.CourseTitle;
                string monthYear = training.MonthYear;
                string trainingInstitution = training.TrainingInstitution;
                string duration = training.Duration;
                string randomNo = Components.GenerateRandomId();
                if (webportals.InsertProffessionalCourses(documentNo, randomNo, courseTitle, monthYear, trainingInstitution, duration))
                {
                    TempData["Success"] = "Proffessional course added successfully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while adding the proffessional course. Please try again later!";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        public ActionResult RemoveProffessionalCourse(string documentNo, string no, string submitted)
        {
            try
            {
                if (submitted == "Yes")
                {
                    TempData["Error"] = "You cannot edit submitted documents";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                if (webportals.RemoveProffessionalCourse(documentNo, no))
                {
                    TempData["Success"] = "Proffessional course deleted successully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while removing the proffessional course. Please try again later!";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        [HttpPost]
        public ActionResult TrainingGaps(Training training)
        {
            string documentNo = Session["DocumentNo"].ToString();
            string submitted = Session["Submitted"].ToString();
            try
            {
                string answer = training.Description;
                string randomNo = Components.GenerateRandomId();
                if (webportals.InsertTrainingGaps(documentNo, randomNo, answer))
                {
                    TempData["Success"] = "Training gap has been added successfully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while adding the training gap. Please try again!";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        public ActionResult RemoveTrainingGaps(string documentNo, string no, string submitted)
        {
            try
            {
                if (submitted == "Yes")
                {
                    TempData["Error"] = "You cannot edit submitted documents";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                if (webportals.RemoveTrainingGaps(documentNo, no))
                {
                    TempData["Success"] = "Training gap deleted successfully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while deleting the training gap. Please try again later";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        [HttpPost]
        public ActionResult TrainingIntervensions(Training training)
        {
            string documentNo = Session["DocumentNo"].ToString();
            string submitted = Session["Submitted"].ToString();
            try
            {
                string randomNo = Components.GenerateRandomId();
                string courseTitle = training.CourseTitle;
                string trainingInstitution = training.TrainingInstitution;
                string monthYear = training.MonthYear;
                string duration = training.Duration;
                decimal estimatedCost = training.EstimatedCost;

                if (webportals.InsertTrainingIntervensions(documentNo, randomNo, courseTitle, trainingInstitution, duration, monthYear, estimatedCost))
                {
                    TempData["Success"] = "Training intervension added successfully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while deleting training intervension. Please try again later!";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        public ActionResult RemoveTrainingIntervensions(string documentNo, string no, string submitted)
        {
            try
            {
                if (submitted == "Yes")
                {
                    TempData["Error"] = "You cannot edit a submitted document"; ;
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                if (webportals.RemoveTrainingInterventions(documentNo, no))
                {
                    TempData["Success"] = "Training intervensions line deleted successfully";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
                else
                {
                    TempData["Error"] = "An error occured while deleting the line. Please try again later";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        [HttpPost]
        public ActionResult TrainingNeedsLines(Training training)
        {
            string documentNo = Session["DocumentNo"].ToString();
            string submitted = Session["Submitted"].ToString();
            try
            {
                var proffessionalQualifications = TrainingHelper.GetProffessionalQualifications();
                var trainingNeedsProffessionalQualifications = TrainingHelper.GetTrainingProffessionalQualifications(documentNo);
                var proffessionalCourses = TrainingHelper.GetProffessionalCourses(documentNo);
                var trainingGaps = TrainingHelper.GetTrainingGaps(documentNo);
                var trainingIntervensions = TrainingHelper.GetTrainingIntervensions(documentNo);
                if (proffessionalQualifications.Count < 1 || trainingNeedsProffessionalQualifications.Count < 1 || proffessionalCourses.Count < 1 || trainingGaps.Count < 1 || trainingIntervensions.Count < 1)
                {
                    TempData["Error"] = "Please ensure that you have provided all the data before submitting";
                    return RedirectToAction("trainingneedslines", new { documentNo, submitted });
                }

                if (webportals.SubmitTrainingNeedsAssement(documentNo))
                {
                    TempData["Success"] = "Training needs assessment has been submitted successfully";
                    return RedirectToAction("trainingneedslisting");
                }
                else
                {
                    TempData["Error"] = "An error occured while submitting the training needs assessment. Please try again later!";
                    return RedirectToAction("trainingneedslines", documentNo, submitted);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainingneedslines", new { documentNo, submitted });
            }
        }

        public ActionResult BtoListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            try
            {
                string username = Session["username"].ToString();
                var trainingBtos = TrainingHelper.GetTrainingBtos(username);
                training.TrainingBtos = trainingBtos;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("btolisting");
            }
            return View(training);
        }

        public ActionResult BtoHeader()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Training training = new Training();
            string username = Session["username"].ToString();
            string staffName = Session["staffName"].ToString();
            var staffDetails = Helper.GetEmployeeDepartmentDetails(username);
            var trainingBtos = TrainingHelper.GetTrainingBtos(username);
            var trainings = TrainingHelper.GetTrainings(username);
            training.Department = staffDetails.Department;
            training.Directorate = staffDetails.Directorate;
            training.StaffNo = username;
            training.StaffName = staffName;
            training.TrainingBtos = trainingBtos;
            training.Trainings = trainings;
            return View(training);
        }

        [HttpPost]
        public ActionResult BtoHeader(Training training)
        {
            try
            {
                string username = Session["username"].ToString();
                string applicationNo = training.ApplicationNo;
                string relevantPoints = training.RelevantPoints;
                string recommendations = training.Recommendations;
                string comments = training.Comments;
                string courseContent = training.CourseContent;
                string fileName1 = training.AttachmentFile.FileName;

                string response = webportals.CreateTrainingBto(username, applicationNo, relevantPoints, recommendations, comments, courseContent);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string btoNumber = responseArr[1];
                        string fileName = training.AttachmentFile.FileName;
                        string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                        if (fileExtension == "pdf" || fileExtension == "doc" || fileExtension == "docx")
                        {
                            string path = Server.MapPath("~/Uploads/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string filename = fileName.Replace(" ", "-");
                            string pathToUpload = Path.Combine(path, btoNumber + filename);
                            training.AttachmentFile.SaveAs(pathToUpload);
                            Stream fs = training.AttachmentFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            byte[] bytes = br.ReadBytes((int)fs.Length);
                            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                            webportals.RegFileUpload(btoNumber, filename, base64String, 52178452);
                            string approval = webportals.OnSendTrainingBackToOfficeForApproval(btoNumber);
                            if (!string.IsNullOrEmpty(approval))
                            {
                                if (approval == "SUCCESS")
                                {
                                    TempData["Success"] = $"Training back to office number {btoNumber} has been submitted successfully";
                                    return RedirectToAction("btolisting");
                                }
                                else
                                {
                                    TempData["Error"] = approval;
                                    return RedirectToAction("btoheader");
                                }
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please upload files with .pdf, .doc, .docx extensions only";
                            return RedirectToAction("btoheader");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "An error occured while creating training back to office header.";
                        return RedirectToAction("btoheader");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("btoheader", "training");
            }
            return View();
        }

        public ActionResult CancelTrainingBto(string documentNo)
        {
            try
            {
                webportals.OnCancellTrainingBackToOfficeForApproval(documentNo);
                TempData["Success"] = "Training back to office has been cancelled successfully";
                return RedirectToAction("btolisting", "training");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("btolisting", "training");
            }
        }

        public ActionResult TrainingEvaluationListing()
        {
            return View();
        }

        public ActionResult TrainingEvaluationHeader()
        {
            return View();
        }
    }
}