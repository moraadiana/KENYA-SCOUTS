using KSAJOBS.Models;
using KSAJOBS.NAVWS;

//using KSAJOBS.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace KSAJOBS.Controllers
{
    public class AccountController : Controller
    {
        Recruitment webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {            
            try
            {
                string username = account.UserName;
                string password = account.Password;
                string response = webportals.RecruitmentUserLogin(username, password);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if(returnMsg == "SUCCESS")
                    {
                        string emailAddress = responseArr[1];
                        Session["username"] = emailAddress;
                        return RedirectToAction("index", "dashboard");
                    }
                    else
                    {
                        TempData["error"] = returnMsg;
                        return RedirectToAction("login", "account");
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("login", "account");
            }
            return View();
        }

        public ActionResult CreateAccount()
        {
            // Initialize the register class
            Register register = new Register();

            // Get a list of all citizenships
            var citizenship = Services.GetCitizenships();

            // Append to the class
            register.CitizenshipList = citizenship;

            // Pass the model to the view
            return View(register);
        }

        [HttpPost]
        public ActionResult CreateAccount(Register register)
        {
            try
            {
                int initials = Convert.ToInt32(register.Initials);
                string firstName = register.FirstName.ToUpper();
                string lastName = register.LastName.ToUpper();
                string middleName = register.MiddleName == null ? string.Empty : register.MiddleName.ToUpper();
                string idNumber = register.IdNumber;
                int gender = Convert.ToInt32(register.Gender);
                int maritalStatus = Convert.ToInt32(register.MaritalStatus);
                string phoneNumber = register.PhoneNumber;
                string postalCode = register.PostalCode;
                string postalAddress = register.PostalAddress.ToUpper();
                DateTime birthDate = register.BirthDate;
                int applicationType = Convert.ToInt32(register.ApplicantType);
                string emailAddress = register.EmailAddress;
                int disability = Convert.ToInt32(register.Disability);
                string disabilityStatus = register.DisabilityStatus == null ? string.Empty : register.DisabilityStatus;
                string password = register.Password;
                string confirmPassword = register.ConfirmPassword;
                string citizenship = register.Citizenship;

                if (password != confirmPassword)
                {
                    TempData["error"] = "Passwords do not match";
                    return RedirectToAction("createaccount", "account");
                }

                string response = webportals.CreateOnlineRecruitmentAccount(initials, firstName, middleName, lastName, idNumber, phoneNumber, gender, maritalStatus, postalCode, postalAddress, birthDate, applicationType, emailAddress, disability, disabilityStatus, password, citizenship);
                if(!string.IsNullOrEmpty(response))
                {
                    string returnMsg = response;
                    if (returnMsg == "SUCCESS")
                    {
                        TempData["success"] = "Account has been created successfully";
                        return RedirectToAction("login", "account");
                    }
                    else if(returnMsg == "FAILED")
                    {
                        TempData["error"] = "An error occured while creating the account. Please try again later";
                        return RedirectToAction("createaccount", "account");
                    }
                    else
                    {
                        TempData["error"] = "Account with email address {emailAddress} already exists. Please proceed to log in";
                        return RedirectToAction("login", "account");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("createaccount", "account");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ResetPassword resetPassword)
        {
            string emailAddress = resetPassword.EmailAddress;
            string response = webportals.RecruitmentUserPassword(emailAddress);
            if (!string.IsNullOrEmpty(response))
            {
                string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                string returnMsg = responseArr[0];
                if(returnMsg == "SUCCESS")
                {
                    string recipient = responseArr[1];
                    string password = responseArr[2];
                    string subject = "AOG Recruitment Portal Password Reset";
                    string body = $"Use the password below to log into your recruitment portal. <br/><br/>Portal Password: <strong>{password}</strong><br/><br/>Do not reply to this email.";
                    Components.SendEmailAlerts(recipient, subject, body);
                    TempData["success"] = $"Password has been sent to your email address {recipient.ToUpper()}";
                    return RedirectToAction("login", "account");
                }
                else
                {
                    TempData["error"] = returnMsg;
                    return RedirectToAction("forgotpassword", "account");
                }
            }
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            return PartialView("_Notification");
        }
    }
}