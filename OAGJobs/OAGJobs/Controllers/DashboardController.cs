using OAGJobs.Models;
using OAGJobs.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGJobs.Controllers
{
    public class DashboardController : Controller
    {
        Recruitment webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Registration registration = new Registration();
            try
            {
                string username = Session["username"].ToString();
                Session["gender"] = "Male";
                if(!webportals.HasUpdatedProfile(username))
                {
                    TempData["Error"] = "Please update your profile before you continue!";
                    return RedirectToAction("profileupdate", "dashboard");
                }
                string response = webportals.GetUserData(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string firstName = responseArr[0];
                    string middleName = responseArr[1];
                    string lastName = responseArr[2];
                    Session["name"] = firstName + ' ' + middleName + ' ' + lastName;
                    registration.FirstName = firstName;
                    registration.MiddleName = middleName;
                    registration.LastName = lastName;
                    registration.IdNumber = username;
                    registration.BirthDate = Convert.ToDateTime(responseArr[3]);
                    registration.KraPin = responseArr[4];
                    Session.Remove("gender");
                    Session["gender"] = responseArr[6];
                    registration.IdNumber = username;
                    registration.PostalCode = responseArr[12];
                    registration.PostalAddress = responseArr[13];
                    registration.MobileNo = responseArr[16];
                    registration.EmailAddress = responseArr[17];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(registration);
        }

        public ActionResult ProfileUpdate()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Registration registration = new Registration();
            try
            {
                string username = Session["username"].ToString();
                var nationalities = Services.GetNationalities();
                var tribes = Services.GetTribes();
                var counties = Services.GetCounties();
                registration.Nationalities = nationalities;
                registration.Tribes = tribes;
                registration.Counties = counties;

                string response = webportals.GetUserData(username);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string firstName = responseArr[0];
                    string middleName = responseArr[1];
                    string lastName = responseArr[2];
                    registration.FirstName = firstName;
                    registration.MiddleName = middleName;
                    registration.LastName = lastName;
                    registration.IdNumber = username;
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(registration);
        }

        [HttpPost]
        public ActionResult ProfileUpdate(Registration registration)
        {
            try
            {
                string idnumber = registration.IdNumber;
                DateTime dateOfBirth = registration.BirthDate;
                string pinNo = registration.KraPin;
                int title = registration.Title;
                int gender = registration.Gender;
                string nationality = registration.Nationality;
                string ethnicity = registration.Tribe;
                string county = registration.County;
                string subCounty = registration.SubCounty;
                string constituency = registration.Constituency;
                string postalAddress = registration.PostalAddress;
                string postalCode = registration.PostalCode;
                string town = registration.Town;
                string telephoneNo = registration.TelephoneNo;
                string mobileNo = registration.MobileNo;
                string emailAddress = registration.EmailAddress;
                string contactPerson = registration.ContactPersonName;
                string contactPersonTel = registration.ContactPersonTel;
                int disability = registration.Disability;
                int maritalStatus = registration.MaritalStatus;
                int ethnicOrigin = registration.EthnicOrigin;
                string natureOfDisability = registration.NatureOfDisability == null ? string.Empty : registration.NatureOfDisability;
                string registrationNo = registration.RegistrationNo == null ? string.Empty : registration.RegistrationNo;

                if (webportals.UpdateUserProfile(idnumber, dateOfBirth, pinNo, title, gender, nationality, ethnicity, county, subCounty, constituency, postalCode, postalAddress, town, telephoneNo, mobileNo, emailAddress, contactPerson, contactPersonTel, disability, natureOfDisability, registrationNo, maritalStatus,ethnicOrigin))
                {
                    TempData["Success"] = "Profile has been updated successfully!";
                    return RedirectToAction("index", "dashboard");
                }
                else
                {
                    TempData["Error"] = "An erorr occured while updating your profile. Please try again later!";
                    return RedirectToAction("profileupdate", "dashboard");
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("profileupdate", "dashboard");
            }
        }

        public ActionResult OpenVacancies()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Job job = new Job();
            try
            {
                string username = Session["username"].ToString();
                if (!webportals.HasUpdatedProfile(username))
                {
                    TempData["Error"] = "Please update your profile before you continue!";
                    return RedirectToAction("profileupdate", "dashboard");
                }
                var list = new List<Job>();
                string advertisedJobs = webportals.GetAdvertisedJobs();
                if (!string.IsNullOrEmpty(advertisedJobs))
                {
                    int counter = 0;
                    string[] advertisedJobsArr = advertisedJobs.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in advertisedJobsArr)
                    {
                        counter++;
                        string[] responsearr = item.Split(strLimiters, StringSplitOptions.None);
                        //string closingDate = responsearr[5];
                        //if (!string.IsNullOrEmpty(closingDate))
                        //{
                        //    if (Convert.ToDateTime(closingDate) > DateTime.Now) continue;
                        //}
                        Job advertisedJob = new Job()
                        {
                            Counter = counter,
                            RefNo = responsearr[0],
                            JobId = responsearr[1],
                            JobTitle = responsearr[2],
                            RequiredPositions = Convert.ToDecimal(responsearr[4]),
                        };
                        list.Add(advertisedJob);
                    }
                    job.AdvertisedJobs = list;
                }
                else
                {
                    job.AdvertisedJobs = list;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(job);
        }

        public ActionResult MyApplications()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Job job = new Job();
            try
            {
                string username = Session["username"].ToString();
                if (!webportals.HasUpdatedProfile(username))
                {
                    TempData["Error"] = "Please update your profile before you continue!";
                    return RedirectToAction("profileupdate", "dashboard");
                }
                var list = new List<Job>();
                string applications = webportals.GetMyApplications(username);
                if (!string.IsNullOrEmpty(applications))
                {
                    string[] applicationsArr = applications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in applicationsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Job application = new Job()
                        {
                            ApplicationNo = responseArr[0],
                            JobId = responseArr[1],
                            JobTitle = responseArr[2],
                            Date = Convert.ToDateTime(responseArr[3]),
                            RefNo = responseArr[4],
                            Status = responseArr[7],
                        };
                        list.Add(application);
                    }
                }
                job.MyApplications = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("myapplications");
            }
            return View(job);
        }

        public ActionResult CompleteApplications()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Job job = new Job();
            try
            {
                string username = Session["username"].ToString();
                if (!webportals.HasUpdatedProfile(username))
                {
                    TempData["Error"] = "Please update your profile before you continue!";
                    return RedirectToAction("profileupdate", "dashboard");
                }
                var list = new List<Job>();
                string applications = webportals.GetMyCompleteApplications(username);
                if (!string.IsNullOrEmpty(applications))
                {
                    string[] applicationsArr = applications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in applicationsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Job application = new Job()
                        {
                            ApplicationNo = responseArr[0],
                            JobId = responseArr[1],
                            JobTitle = responseArr[2],
                            Date = Convert.ToDateTime(responseArr[3]),
                            RefNo = responseArr[4],
                            Status = responseArr[7],
                        };
                        list.Add(application);
                    }
                }
                job.MyApplications = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("completeapplications");
            }
            return View(job);
        }

        public JsonResult GetSubCounties(string county)
        {
            var subCounties = Services.GetSubCounties(county);
            return Json(subCounties, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConstituences(string subCounty)
        {
            var constituencies = Services.GetConstituencies(subCounty);
            return Json(constituencies, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("index", "login");
        }
    }
}