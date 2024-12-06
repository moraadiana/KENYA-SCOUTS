using OAGJobs.Models;
using OAGJobs.NAVWS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace OAGJobs.Controllers
{
    public class JobApplicationController : Controller
    {
        Recruitment webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult GeneralInformation(string jobId, string refNo)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Registration registration = new Registration();
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.GetUserData(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string firstName = responseArr[0];
                    string middleName = responseArr[1];
                    string lastName = responseArr[2];
                    registration.FirstName = firstName;
                    registration.MiddleName = middleName;
                    registration.LastName = lastName;
                    registration.IdNumber = username;
                    registration.BirthDate = Convert.ToDateTime(responseArr[3]);
                    registration.KraPin = responseArr[4];
                    registration.IdNumber = username;
                    registration.PostalCode = responseArr[12];
                    registration.PostalAddress = responseArr[13];
                    registration.MobileNo = responseArr[16];
                    registration.EmailAddress = responseArr[17];
                    Session["jobId"] = jobId;
                    Session["refNo"] = refNo;
                    string jobTitle = webportals.GetJobTitle(jobId);
                    Session["jobTitle"] = jobTitle;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(registration);
        }

        [HttpPost]
        public ActionResult GeneralInformation()
        {
            string jobId = Session["jobId"].ToString();
            string refNo = Session["refNo"].ToString();
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.SubmitJobApplication(username, jobId, refNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS" || returnMsg == "Record Exists")
                    {
                        string applicationNo = responseArr[1];
                        Session["ApplicationNo"] = applicationNo;
                        return RedirectToAction("minimumrequirements");
                    }
                    else
                    {
                        TempData["Error"] = returnMsg;
                        return RedirectToAction("generalinformation", new { jobId, refNo });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("generalinformation", new { jobId, refNo });
            }
            return View();
        }

        public ActionResult MinimumRequirements()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            string jobId = Session["jobId"].ToString();
            string refNo = Session["refNo"].ToString();
            string applicationNo = Session["ApplicationNo"].ToString();
            Applicant applicant = new Applicant();
            try
            {
                //var minimumRequirements = Services.GetMinimumRequirements(applicationNo,jobId,refNo);
                var submittedRequirements = Services.GetSubmittedMinimumRequirements(jobId, refNo);
                var qualificationTypes = Services.GetQualificationTypes(jobId);
                //applicant.MinimumRequirements = minimumRequirements;
                applicant.SubmittedMinimumRequirements = submittedRequirements;
                applicant.QualificationTypes = qualificationTypes;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("minimumrequirements", "jobapplication");
            }
            return View(applicant);
        }

        [HttpPost]
        public ActionResult SubmitRequirements(Applicant applicant)
        {
            try
            {
                string jobId = Session["jobId"].ToString();
                string refNo = Session["refNo"].ToString();
                string applicationNo = Session["ApplicationNo"].ToString();
                string code = applicant.QualificationType;
                string description = applicant.QualificationCode;
                if (applicant.AttachmentFile != null || applicant.AttachmentFile.ContentLength > 0)
                {
                    string fileName = applicant.AttachmentFile.FileName;
                    string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    if (fileExtension == "pdf")
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string pathToUpload = Path.Combine(path, applicationNo.Replace("/", "") + fileName);
                        if (System.IO.File.Exists(pathToUpload))
                        {
                            System.IO.File.Delete(pathToUpload);
                        }
                        applicant.AttachmentFile.SaveAs(pathToUpload);
                        Stream fs = applicant.AttachmentFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        webportals.RegFileUpload(applicationNo, fileName.ToUpper(), base64String, 52179114);
                    }
                    else
                    {
                        TempData["Error"] = "Please upload files with .pdf extensions only.";
                        return RedirectToAction("minimumrequirements", "jobapplication");
                    }
                    webportals.UpdateEmployeeMinimuRequirement(applicationNo, refNo, jobId, 2, code, description);
                }
                else
                {
                    webportals.UpdateEmployeeMinimuRequirement(applicationNo, refNo, jobId, 1, code, description);
                }
                TempData["Success"] = "Minimum requirements added successfully!";
                return RedirectToAction("minimumrequirements", "jobapplication");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("minimumrequirements", "jobapplication");
            }
        }

        public ActionResult Qualifications()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Applicant applicant = new Applicant();
            try
            {
                string applicationNo = Session["username"].ToString();
                var qualifications = Services.GetQualifications(applicationNo);
                var areasOfStudy = Services.GetAreasOfStudy();
                applicant.ApplicantQualifications = qualifications;
                applicant.AreasOfStudy = areasOfStudy;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("qualifications", "jobapplication");
            }
            return View(applicant);
        }

        [HttpPost]
        public ActionResult SubmitQualification(Applicant applicant)
        {
            try
            {
                string applicationNo = Session["username"].ToString();
                string institution = applicant.Institution;
                string areaOfStudy = applicant.AreaOfStudy;
                string areaOfSpecialization = applicant.AreaOfSpecialization;
                string award = applicant.Award;
                string course = applicant.Course;
                string grade = applicant.Grade;
                DateTime dateFrom = applicant.DateFrom;
                DateTime dateTo = applicant.DateTo;
                if (applicant.AttachmentFile != null || applicant.AttachmentFile.ContentLength > 0)
                {
                    string fileName = applicant.AttachmentFile.FileName;
                    string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    if (fileExtension == "pdf")
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string pathToUpload = Path.Combine(path, applicationNo.Replace("/", "") + fileName);
                        if (System.IO.File.Exists(pathToUpload))
                        {
                            System.IO.File.Delete(pathToUpload);
                        }
                        applicant.AttachmentFile.SaveAs(pathToUpload);
                        Stream fs = applicant.AttachmentFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        webportals.RegFileUpload(applicationNo, fileName.ToUpper(), base64String, 52179114);
                    }
                    else
                    {
                        TempData["Error"] = "Please upload files with .pdf extensions only.";
                        return RedirectToAction("qualifications", "jobapplication");
                    }
                }
                if (webportals.InsertApplicantQualification(applicationNo, areaOfStudy, areaOfSpecialization, award, course, grade, institution, dateFrom, dateTo))
                {
                    TempData["Success"] = "Qualification added successfully!";
                    return RedirectToAction("qualifications", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while adding qualification. Please try again later!";
                    return RedirectToAction("qualifications", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("qualifications", "jobapplication");
            }
        }

        public ActionResult RemoveQualification(string id)
        {
            try
            {
                if (webportals.RemoveApplicantQualifications(id))
                {
                    TempData["Success"] = "Qualification has been deleted successfully!";
                    return RedirectToAction("qualifications", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while removing qualification. Please try again later!";
                    return RedirectToAction("qualification", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("qualification", "jobapplication");
            }
        }

        public ActionResult NavigateProffessionalQual()
        {
            string applicationNo = Session["username"].ToString();
            var qualifications = Services.GetQualifications(applicationNo);
            return RedirectToAction("proffessionalqualifications", "jobapplication");
            //if(qualifications.Count > 0)
            //{
            //}
            //else
            //{
            //    TempData["Error"] = "Please add academic qualifications before you continue!";
            //    return RedirectToAction("qualifications", "jobapplication");
            //}
        }

        public ActionResult ProffessionalQualifications()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Applicant applicant = new Applicant();
            try
            {
                string applicationNo = Session["username"].ToString();
                var qualifications = Services.GetProfessionalQualifications(applicationNo);
                applicant.ApplicantProfessionalQualifications = qualifications;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("proffessionalqualifications", "jobapplication");
            }
            return View(applicant);
        }

        [HttpPost]
        public ActionResult SubmitProfessionalQualification(Applicant applicant)
        {
            try
            {
                string applicationNo = Session["username"].ToString();
                string institution = applicant.Institution;
                string award = applicant.Award;
                string subject = applicant.Specialization;
                string grade = applicant.Grade;
                DateTime dateFrom = applicant.DateFrom;
                DateTime dateTo = applicant.DateTo;

                if (applicant.AttachmentFile != null || applicant.AttachmentFile.ContentLength > 0)
                {
                    string fileName = applicant.AttachmentFile.FileName;
                    string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    if (fileExtension == "pdf")
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string pathToUpload = Path.Combine(path, applicationNo.Replace("/", "") + fileName);
                        if (System.IO.File.Exists(pathToUpload))
                        {
                            System.IO.File.Delete(pathToUpload);
                        }
                        applicant.AttachmentFile.SaveAs(pathToUpload);
                        Stream fs = applicant.AttachmentFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        webportals.RegFileUpload(applicationNo, fileName.ToUpper(), base64String, 52179114);
                    }
                    else
                    {
                        TempData["Error"] = "Please upload files with .pdf extensions only.";
                        return RedirectToAction("qualifications", "jobapplication");
                    }
                }

                if (webportals.InsertProfessionalQualifications(applicationNo, institution, award, subject, grade, dateFrom, dateTo))
                {
                    TempData["Success"] = "Professional qualification added successfully";
                    return RedirectToAction("proffessionalqualifications", "jobapplication");
                }
                {
                    TempData["Error"] = "An error occured while adding professional qualification. Please try again later!";
                    return RedirectToAction("proffessionalqualifications", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("proffessionalqualifications", "jobapplication");
            }
        }

        public ActionResult RemoveProfessionalQualification(string id)
        {
            try
            {
                if (webportals.RemoveProfessionalQualifications(id))
                {
                    TempData["Success"] = "Professional qualification has been deleted successfully!";
                    return RedirectToAction("proffessionalqualifications", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while deleting professional qualification!";
                    return RedirectToAction("proffessionalqualifications", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("proffessionalqualifications", "jobapplication");
            }
        }

        public ActionResult NavigateProffessionalBody()
        {
            string applicationNo = Session["username"].ToString();
            var qualifications = Services.GetProfessionalQualifications(applicationNo);
            return RedirectToAction("proffessionalbodies", "jobapplication");
        }

        public ActionResult ProffessionalBodies()
        {
            Applicant applicant = new Applicant();
            try
            {
                string applicationNo = Session["username"].ToString();
                var bodies = Services.GetProfessionalBodies(applicationNo);
                applicant.ProffessionalBodies = bodies;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(applicant);
        }

        public ActionResult SubmitProfessionalBody(Applicant applicant)
        {
            try
            {
                string applicationNo = Session["username"].ToString();
                string proffesionalBody = applicant.ProffessionalBody;
                string membershipNo = applicant.MembershipNo;
                string membershipType = applicant.MembershipType;
                DateTime renewalDate = applicant.DateOfRenewal;
                if (webportals.InsertProfessionalBodies(applicationNo, proffesionalBody, membershipNo, membershipType, renewalDate))
                {
                    TempData["Success"] = "Proffessional body has been inserted successfully!";
                    return RedirectToAction("proffessionalbodies", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while adding proffessional body. Please try again later!";
                    return RedirectToAction("proffessionalbodies", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("proffessionalbodies", "jobapplication");
            }
        }

        public ActionResult RemoveProfessionalBody(string id)
        {
            try
            {
                if (webportals.RemoveProfessionalBody(id))
                {
                    TempData["Success"] = "Proffessional body has been removed successfully!";
                    return RedirectToAction("proffessionalbodies", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while removing proffessional body!";
                    return RedirectToAction("proffessionalbodies", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("proffessionalbodies", "jobapplication");
            }
        }

        public ActionResult NavigateNok()
        {
            string applicationNo = Session["username"].ToString();
            var bodies = Services.GetProfessionalBodies(applicationNo);
            return RedirectToAction("nextofkin", "jobapplication");
        }

        public ActionResult NextOfKin()
        {
            Applicant applicant = new Applicant();
            try
            {
                string applicationNo = Session["username"].ToString();
                var relatives = Services.GetRelationships();
                var nextOfKins = Services.GetNextOfKins(applicationNo);
                applicant.ApplicantNok = nextOfKins;
                applicant.Relationships = relatives;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(applicant);
        }

        [HttpPost]
        public ActionResult SubmitNextOfkin(Applicant applicant)
        {
            try
            {
                string applicationNo = Session["username"].ToString();
                string name = applicant.NokName;
                string address = applicant.NokRelationship;
                string tel = applicant.NokTel;
                string relationship = applicant.NokRelationship;
                if (webportals.InsertNextOfKin(applicationNo, name, address, tel, relationship))
                {
                    TempData["Success"] = "Next of kin added successfully!";
                    return RedirectToAction("nextofkin", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while adding next of kin. Please try aagin later!";
                    return RedirectToAction("nextofkin", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("nextofkin", "jobapplication");
            }
        }

        public ActionResult RemoveNok(string id)
        {
            try
            {
                if (webportals.RemoveNextOfKin(id))
                {
                    TempData["Success"] = "Next of kin has been removed successfully";
                    return RedirectToAction("nextofkin", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while removing next of kin. Please try again later!";
                    return RedirectToAction("nextofkin", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("nextofkin", "jobapplication");
            }
        }

        public ActionResult NavigateReferees()
        {
            string applicationNo = Session["username"].ToString();
            var nextOfKins = Services.GetNextOfKins(applicationNo);
            return RedirectToAction("referees", "jobapplication");
        }

        public ActionResult Referees()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Applicant applicant = new Applicant();
            try
            {
                string applicationNo = Session["username"].ToString();
                var referees = Services.GetApplicantReferees(applicationNo);
                applicant.ApplicantReferees = referees;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(applicant);
        }

        [HttpPost]
        public ActionResult SubmitReferee(Applicant applicant)
        {
            try
            {
                string name = applicant.RefereeName;
                string institution = applicant.RefereeInstitution;
                string designation = applicant.RefereeDesignation;
                string email = applicant.RefereeEmail;
                string phone = applicant.RefereePhone;
                string address = applicant.RefereeAddress;
                string postCode = applicant.RefereePostCode;
                string city = applicant.RefereeCity;
                string period = applicant.RefereePeriod;
                string applicationNo = Session["username"].ToString();
                if (webportals.InsertApplicantReferees(applicationNo, name, designation, institution, address, phone, email, postCode, city, period))
                {
                    TempData["Success"] = "Referee has been added successfully";
                    return RedirectToAction("referees", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while adding referee. Please try again later.";
                    return RedirectToAction("referees", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("referees", "jobapplication");
            }
        }

        public ActionResult RemoveReferee(string id)
        {
            try
            {
                if (webportals.RemoveApplicantReferee(id))
                {
                    TempData["Success"] = "Referee has been removed successfully";
                    return RedirectToAction("referees", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while removing referee. Please try again later!";
                    return RedirectToAction("referees", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("referees", "jobapplication");
            }
        }

        public ActionResult NavigateAttachments()
        {
            string applicationNo = Session["username"].ToString();
            var referees = Services.GetApplicantReferees(applicationNo);
            return RedirectToAction("attachments", "jobapplication");
        }

        public ActionResult Attachments()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Applicant applicant = new Applicant();
            try
            {
                string applicationNo = Session["username"].ToString();
                var attachments = Services.GetApplicantAttachments(applicationNo);
                applicant.ApplicantAttachments = attachments;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(applicant);
        }

        [HttpPost]
        public ActionResult SubmitAttachment(Applicant applicant)
        {
            try
            {
                string applicationNo = Session["username"].ToString();
                string fileName = applicant.AttachmentFile.FileName;
                string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                if (fileExtension == "pdf")
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string pathToUpload = Path.Combine(path, applicationNo.Replace("/", "") + fileName);
                    if (System.IO.File.Exists(pathToUpload))
                    {
                        System.IO.File.Delete(pathToUpload);
                    }
                    applicant.AttachmentFile.SaveAs(pathToUpload);
                    Stream fs = applicant.AttachmentFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    byte[] bytes = br.ReadBytes((int)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    webportals.RegFileUpload(applicationNo, fileName.ToUpper(), base64String, 52179114);
                    TempData["Success"] = "Document has been uploaded successfully";
                    return RedirectToAction("attachments", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "Please upload files with .pdf extensions only.";
                    return RedirectToAction("attachments", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("attachments", "jobapplication");
            }
        }

        public ActionResult RemoveAttachment(string id)
        {
            try
            {
                if (webportals.DeleteAttachment(id))
                {
                    TempData["Success"] = "Document has been deleted successfully";
                    return RedirectToAction("attachments", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while deleting document. Please try again later!";
                    return RedirectToAction("attachments", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("attachments", "jobapplication");
            }
        }

        public ActionResult Complete()
        {
            try
            {
                string applicationNo = Session["ApplicationNo"].ToString();
                if (!webportals.SubmitApplication(applicationNo))
                {
                    TempData["Error"] = "An error occured while submitting the application. Please try again later!";
                    return RedirectToAction("attachments", "jobapplication");
                }
                string jobTitle = Session["jobTitle"].ToString();
                string recipient = Session["username"].ToString();
                string subject = "Office of the Auditor General Job Application Alert";
                string body = $"Hello " +
                    $"<br/><br/>" +
                    $"You have successfully initiated a job application for <b>{jobTitle}</b> at the Office of the Auditor General." +
                    $"<br/><br/>" +
                    $"Your application reference number is: <b>{applicationNo}</b>";
                Components.SendMyEmail(recipient, subject, body);
                TempData["success"] = "Job application has been submitted successfully.";
                return RedirectToAction("completeapplications", "dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("attachments", "jobapplication");
            }
        }

        public ActionResult Trainings()
        {
            Applicant applicant = new Applicant();
            try
            {
                string username = Session["username"].ToString();
                var trainings = Services.GetApplicantTrainings(username);
                applicant.ApplicantTrainings = trainings;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(applicant);
        }

        [HttpPost]
        public ActionResult SubmitTraining(Applicant applicant)
        {
            try
            {
                string applicationNo = Session["username"].ToString();
                string institution = applicant.Institution;
                string course = applicant.Course;
                string duration = applicant.Duration;
                int year = applicant.Year;
                if (webportals.InsertTrainingAttended(applicationNo, institution, course, duration, year))
                {
                    TempData["Success"] = "Trainings attended inserted successfully";
                    return RedirectToAction("trainings", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while adding trainings attended. Please try again later!";
                    return RedirectToAction("trainings", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainings", "jobapplication");
            }
        }

        public ActionResult RemoveTraining(string id)
        {
            try
            {
                if (webportals.RemoveTrainingAttended(id))
                {
                    TempData["Success"] = "Training attended has been deleted successfully";
                    return RedirectToAction("trainings", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while deleting trainigs attended. Please try again later!";
                    return RedirectToAction("trainings", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("trainings", "jobapplication");
            }
        }

        public ActionResult EmploymentDetails()
        {
            Applicant applicant = new Applicant();
            try
            {
                string username = Session["username"].ToString();
                var details = Services.GetEmploymentDetails(username);
                applicant.ApplicantEmploymentDetails = details;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(applicant);
        }

        public ActionResult SubmitEmploymentDetails(Applicant applicant)
        {
            try
            {
                string username = Session["username"].ToString();
                string designation = applicant.Designation;
                string jobGrade = applicant.JobGrade;
                string organization = applicant.Institution;
                decimal salary = applicant.Salary;
                DateTime dateFrom = applicant.DateFrom;
                DateTime dateTo = applicant.DateTo;

                if (webportals.InsertEmploymentDetails(username, designation, jobGrade, organization, salary, dateFrom, dateTo))
                {
                    TempData["Success"] = "Employment history has been added successfully!";
                    return RedirectToAction("employmentdetails", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error occured while adding employment details. Please try again later!";
                    return RedirectToAction("employmentdetails", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("employmentdetails", "jobapplication");
            }
        }

        public ActionResult RemoveEmploymentDetail(string id)
        {
            try
            {
                if (webportals.RemoveEmploymentDetails(id))
                {
                    TempData["Success"] = "Employment details removed successfully!";
                    return RedirectToAction("employmentdetails", "jobapplication");
                }
                else
                {
                    TempData["Error"] = "An error has occured while removing employment details. Please try again later!";
                    return RedirectToAction("employmentdetails", "jobapplication");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("employmentdetails", "jobapplication");
            }
        }
    }
}