using KSAJOBS.Models;
using KSAJOBS.NAVWS;
using KSAJOBS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KSAJOBS.Controllers
{
    public class JobApplicationController : Controller
    {
        Recruitment webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult GeneralInformation(string jobId, string refNo)
        {
            if (Session["username"] == null) return RedirectToAction("index", "home");
            string username = Session["username"].ToString();
            string response = webportals.RecruitmentUserDetails(username);
            if (!string.IsNullOrEmpty(response))
            {
                string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                string returnMsg = responseArr[0];
                if (returnMsg == "SUCCESS")
                {
                    ViewBag.initials = responseArr[1];
                    ViewBag.firstName = responseArr[2];
                    ViewBag.middleName = responseArr[3];
                    ViewBag.lastName = responseArr[4];
                    ViewBag.idNumber = responseArr[5];
                    ViewBag.phone = responseArr[6];
                    ViewBag.gender = responseArr[7];
                    ViewBag.maritalStatus = responseArr[8];
                    ViewBag.postalCode = responseArr[9];
                    ViewBag.postalAddress = responseArr[10];
                    ViewBag.birthYear = responseArr[11];
                    ViewBag.email = responseArr[12];
                    ViewBag.citizenship = responseArr[13];
                    ViewBag.jobId = jobId;
                    ViewBag.jobRefNo = refNo;
                   string jobTitle = Services.GetJobDescription(jobId);
                    ViewBag.jobTitle = jobTitle;
                    Session["jobId"] = jobId;
                    Session["jobTitle"] = jobTitle;
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult GeneralInformation(ApplicantInformation information)
        {
            try
            {
                string emailAddress = information.EmailAddress;
                string jobId = information.JobId;
                string refNo = information.JobRefNo;
                string response = webportals.SubmitJobApplication(emailAddress, jobId, refNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS" || returnMsg == "Record Exists")
                    {
                        string applicationNo = responseArr[1];
                        Session["ApplicationNo"] = applicationNo;
                        return RedirectToAction("qualifications");
                    }
                    else
                    {
                        TempData["error"] = "An error occured while submitting the application. Please try again later.";
                        return RedirectToAction("generalinformation", new { jobId,refNo});
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("generalinformation");
            }
            return View();
        }

        public ActionResult Qualifications()
        {
            if (Session["ApplicationNo"] == null) return RedirectToAction("generalinformation");

            // Declare the qualification class instance
            Qualification qualifications = new Qualification();

            // Get the application no
            string applicationNo = Session["ApplicationNo"].ToString();

            // Get the list of all qualifications
            var applicantQualifications = Services.GetApplicantQualifications(applicationNo);

            // Append the qualification list
            qualifications.ApplicantQualifications = applicantQualifications;

            // Pass the object to the view
            return View(qualifications);
        }

        [HttpPost]
        public ActionResult Qualifications(Qualification qualification)
        {
            try
            {
                string applicationNo = Session["ApplicationNo"].ToString();
                string qualificationType = qualification.QualificationType;
                string qualificationCode = qualification.QualificationCode;
                string institution = qualification.Institution;
                DateTime dateFrom = qualification.DateFrom;
                DateTime dateTo = qualification.DateTo;
                int type = Convert.ToInt32(qualification.Type);

                string response = webportals.HRMApplicantQualifications(applicationNo, qualificationType, qualificationCode, institution, dateFrom, dateTo, type);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["success"] = "Qualification added successfully";
                        return RedirectToAction("qualifications");
                    }
                    else if (response == "FAILED")
                    {
                        TempData["error"] = "An error occured while adding qualification. Please try again later";
                        return RedirectToAction("qualifications");
                    }
                    else
                    {
                        TempData["error"] = response;
                        return RedirectToAction("qualifications");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("qualifications");
            }
            return View();
        }

        public ActionResult RemoveQualification(string applicationNo, string qualificationCode)
        {
            try
            {
                string response = webportals.HRMRemoveApplicantQualification(applicationNo, qualificationCode);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["success"] = "Qualification removed successfully";
                        return RedirectToAction("qualifications");
                    }
                    else
                    {
                        TempData["error"] = "An error occured while deleting qualification. Please try again later";
                        return RedirectToAction("qualifications");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("qualifications");
            }
            return View();
        }

        public ActionResult NavigateReferees()
        {
            string applicationNo = Session["ApplicationNo"].ToString();
            //var applicantQualifications = Services.GetApplicantQualifications(applicationNo);
            return RedirectToAction("referees");
            //if (applicantQualifications.Count >= 2) return RedirectToAction("referees");
            //else
            //{
            //    TempData["error"] = "You must add atleast two qualifications";
            //    return RedirectToAction("qualifications");
            //}
        }

        public ActionResult Referees()
        {
            Referee referee = new Referee();
            string applicationNo = Session["ApplicationNo"].ToString();
            var applicantReferees = Services.GetApplicantReferee(applicationNo);
            referee.ApplicantReferees = applicantReferees;
            return View(referee);
        }

        [HttpPost]
        public ActionResult Referees(Referee referee)
        {
            try
            {
                string applicationNo = Session["ApplicationNo"].ToString();
                string refereeName = referee.RefereeName;
                string refereeDesignation = referee.RefereeDesignation;
                string refereeInstitution = referee.RefereeInstitution;
                string refereeEmail = referee.RefereeEmail;
                string refereeAddress = referee.RefereeAddress;
                string refereePhone = referee.RefereePhone;

                string response = webportals.HRMApplicantReferees(applicationNo, refereeName, refereeDesignation, refereeInstitution, refereeAddress, refereePhone, refereeEmail);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["success"] = "Referee has been added sucessfully";
                        return RedirectToAction("referees");
                    }
                    else if (response == "FAILED")
                    {
                        TempData["error"] = "An error occured while adding the referee. Please try again later.";
                        return RedirectToAction("referees");
                    }
                    else
                    {
                        TempData["error"] = response;
                        return RedirectToAction("referees");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("referees");
            }
            return View();
        }

        public ActionResult RemoveReferee(string email)
        {
            try
            {
                string applicationNo = Session["ApplicationNo"].ToString();
                string response = webportals.HRMRemoveApplicantReferee(applicationNo, email);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["success"] = "Referee has been deleted successfully";
                        return RedirectToAction("referees");
                    }
                    else
                    {
                        TempData["error"] = "An error occured while deleting the referee. Please try again later";
                        return RedirectToAction("referees");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("referees");
            }
            return View();
        }

        public ActionResult NavigateHobbies()
        {
            string applicationNo = Session["ApplicationNo"].ToString();
            var referees = Services.GetApplicantReferee(applicationNo);
            if (referees.Count >= 2) return RedirectToAction("hobbies");
            else
            {
                TempData["error"] = "You must have atleast 2 or more referees";
                return RedirectToAction("referees");
            }
        }

        public ActionResult Hobbies()
        {
            string applicationNo = Session["ApplicationNo"].ToString();
            Hobby hobby = new Hobby();
            var applicantHobbies = Services.GetApplicantHobbies(applicationNo);
            hobby.ApplicantHobbies = applicantHobbies;
            return View(hobby);
        }

        [HttpPost]
        public ActionResult Hobbies(Hobby hobby)
        {
            try
            {
                string applicationNo = Session["ApplicationNo"].ToString();
                string userHobby = hobby.ApplicantHobby;
                string response = webportals.HRMApplicantHobby(applicationNo, userHobby);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["success"] = "Hobby added successfully.";
                        return RedirectToAction("hobbies");
                    }
                    else if (response == "FAILED")
                    {
                        TempData["error"] = "An error occured while adding hobby. Please try again later";
                        return RedirectToAction("hobbies");
                    }
                    else
                    {
                        TempData["error"] = response;
                        return RedirectToAction("hobbies");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("hobbies");
            }
            return View();
        }

        public ActionResult RemoveHobby(string hobby)
        {
            try
            {
                string applicationNo = Session["ApplicationNo"].ToString();
                string response = webportals.HRMRemoveApplicantHobby(applicationNo, hobby);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        TempData["success"] = "Hobby deleted successfully.";
                        return RedirectToAction("hobbies");
                    }
                    else
                    {
                        TempData["error"] = "An error occured while deleting hobby. Please try again later";
                        return RedirectToAction("hobbies");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("hobbies");
            }
            return View();
        }

        public ActionResult NavigateAttachments()
        {
            string applicationNo = Session["ApplicationNo"].ToString();
            var hobbies = Services.GetApplicantHobbies(applicationNo);
            if (hobbies.Count >= 3) return RedirectToAction("attachments");
            else
            {
                TempData["error"] = "You must add atleast 3 hobbies";
                return RedirectToAction("hobbies");
            }
        }

        public ActionResult Attachments()
        {
            string applicationNo = Session["ApplicationNo"].ToString();
            var applicantAttachments = Services.GetApplicantAttachments(applicationNo);
            return View(applicantAttachments);
        }
        [HttpPost]
        public ActionResult Attachments(HttpPostedFileBase attachmentFile)
        {
            string fileName = attachmentFile.FileName.Replace(" ", "-");
            string documentNo = Session["ApplicationNo"].ToString();
            string username = Session["username"].ToString();
            string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
            if (fileExtension == "pdf" || fileExtension == "docx" || fileExtension == "doc" || fileExtension == "png" || fileExtension == "jpeg" || fileExtension == "jpg")
            {
                string strPath = Server.MapPath("~/Attachments");
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                string pathToUpload = Path.Combine(strPath, documentNo + fileName.ToUpper());
                attachmentFile.SaveAs(pathToUpload);
                webportals.SaveMemoAttchmnts(documentNo, pathToUpload, fileName.ToUpper(), username);
                Stream fs = attachmentFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((int)fs.Length);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                webportals.RegFileUpload(documentNo, fileName.ToUpper(), base64String, 52178490);
                TempData["success"] = "Document uploaded successfully";
                return RedirectToAction("attachments");
            }
            else
            {
                TempData["error"] = "Please upload files with .pdf, .docx, .png, .jpg and .jpeg extensions only.";
                return RedirectToAction("attachments");
            }
        }

        public ActionResult RemoveAttachment(string systemId)
        {
            try
            {
                string description = webportals.GetAttachmentFileName(systemId);
                string applicationNo = Session["ApplicationNo"].ToString();
                string deleteAttachment = webportals.DeleteMemoAttachment(description, applicationNo);
                string fileName = description.Split('.')[0].ToString();
                string deleteMemoAttachment = webportals.DeleteDocumentAttachment(fileName, applicationNo);
                if(deleteAttachment == "SUCCESS" && deleteMemoAttachment == "SUCCESS")
                {
                    TempData["success"] = "Docuement attachment deleted successfully.";
                    return RedirectToAction("attachments");
                }
                else
                {
                    TempData["error"] = "An error occured while deleting attachment. Please try again later.";
                    return RedirectToAction("attachments");
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("attachments");
            }
        }

        public ActionResult Complete()
        {
            try
            {
                string applicationNo = Session["ApplicationNo"].ToString();
                var attachments = Services.GetApplicantAttachments(applicationNo);
                //if(attachments.Count <= 0)
                //{
                //    TempData["error"] = "Please upload attachments before proceeding";
                //    return RedirectToAction("attachments");
                //}
                string jobTitle = Session["jobTitle"].ToString();
                string recipient = Session["username"].ToString();
                string subject = "KSA Job Application Alert";
                string body = $"Hello {webportals.RecruitmentUserNames(recipient)}" +
                    $"<br/><br/>" +
                    $"You have successfully initiated a job application for <b>{jobTitle}</b> at the Kenya Scouts." +
                    $"<br/>" +
                    $"Your application reference number is: <b>{applicationNo}</b>";
                Components.SendEmailAlerts(recipient, subject, body);
                TempData["success"] = "Job application has been submitted successfully.";
                return RedirectToAction("applications", "dashboard");
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("attachments");
            }
        }

        public JsonResult QualificationType()
        {
            string jobId = Session["jobId"].ToString();
            var json = Services.GetQualificationTypes(jobId);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QualificationCode(string qualificationCode)
        {
            string jobId = Session["jobId"].ToString();
            var json = Services.GetQualificationCodes(jobId,qualificationCode);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            return PartialView("_Notification");
        }
    }
}