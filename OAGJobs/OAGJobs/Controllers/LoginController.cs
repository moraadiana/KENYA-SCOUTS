using OAGJobs.Models;
using OAGJobs.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace OAGJobs.Controllers
{
    public class LoginController : Controller
    {
        Recruitment webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Registration registration)
        {
            try
            {
                string idNumber = registration.IdNumber;
                string password = registration.Password;

                if(!webportals.AccountExists(idNumber))
                {
                    TempData["Error"] = $"Account with ID Number {idNumber} does not exist!";
                    return RedirectToAction("index", "login");
                }

                string response = webportals.RecruitmentUserLogin(idNumber, password);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string username = responseArr[1];
                        Session["username"] = username;
                        return RedirectToAction("index", "dashboard");
                    }
                    else
                    {
                        TempData["Error"] = returnMsg;
                        return RedirectToAction("index", "login");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "login");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(Registration registration)
        {
            try
            {
                string idNumber = registration.IdNumber;
                if (!webportals.AccountExists(idNumber))
                {
                    TempData["Error"] = "Invalid Id Number. Please try again later";
                    return RedirectToAction("forgotpassword", "login");
                }
                return RedirectToAction("resetpassword", "login", new { idNumber });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("forgotpassword", "login");
            }
        }

        public ActionResult ResetPassword(string idNumber)
        {
            if (Request.QueryString["idNumber"] == null) return RedirectToAction("index", "login");
            Session["IdNumber"] = idNumber;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Registration registration)
        {
            string idNumber = Session["IdNumber"].ToString();
            try
            {
                string password = registration.Password;
                if (!ValidPassword(password))
                {
                    TempData["Error"] = "Password must be atleast 8 characters, contain atleast one lowercase letter, one uppercase letter, a number and a special character";
                    return RedirectToAction("resetpassword", "login", new { idNumber });
                }

                if (webportals.UpdateUserPassword(idNumber, password))
                {
                    TempData["Success"] = "Password has been updated successfully";
                    return RedirectToAction("index", "login");
                }
                else
                {
                    TempData["Error"] = "An error occured while updating password. Please try again later!";
                    return RedirectToAction("resetpassword", "login", new { idNumber });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("resetpassword", "login", new { idNumber });
            }
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(Registration registration)
        {
            try
            {
                string idNumber = registration.IdNumber;
                string firstName = registration.FirstName;
                string middleName = registration.MiddleName == null ? string.Empty : registration.MiddleName;
                string lastName = registration.LastName;
                string password = registration.Password;
                string email = registration.EmailAddress;

                if (webportals.AccountExists(idNumber))
                {
                    TempData["Error"] = $"Account with Id Number {idNumber} already exists!";
                    return RedirectToAction("createaccount", "login");
                }

                if(!ValidPassword(password))
                {
                    TempData["Error"] = "Password must be atleats 8 characters long, not more than 20 characters, atleast one lowercase letter, one uppercase letter, a number and a special character!";
                    return RedirectToAction("createaccount", "login");
                }

                if(webportals.CreateOnlineAccount(idNumber, firstName, middleName, lastName, password, email))
                {
                    string subject = "Office of the Auditor General Recruitment Account";
                    string body = $"Dear {firstName} {middleName} {lastName},." +
                        $"<br/><br/>" +
                        $"You have successfully created an account at the office of the Auditor General." +
                        $"<br/><br/>" +
                        $"Regards, Administrator";
                    Components.SendMyEmail(email,subject,body);
                    TempData["Success"] = "Account has been created successfully!";
                    return RedirectToAction("index", "login");
                }
                else
                {
                    TempData["Error"] = "An error occured while creating account. Please try again later!";
                    return RedirectToAction("createaccount", "login");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("createaccount", "login");
            }
        }

        private bool ValidPassword(string password)
        {
            bool valid = false;
            try
            {
                string pattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,20}$";
                if (Regex.IsMatch(password, pattern))
                {
                    valid = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return valid;
        }
    }
}