using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OAGStaff.Controllers
{
    public class LoginController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                string username = user.UserName;
                string password = user.Password;

                if (!webportals.CheckValidStaffNo(username))
                {
                    TempData["Error"] = "Invalid staff number";
                    return RedirectToAction("index", "login");
                }

                if (ChangedPassword(username))
                {
                    return RedirectToAction("loginforchangedpassword", "login", new { username, password });
                }
                else
                {
                    return RedirectToAction("loginforunchangedpassword", "login", new { username });
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View();
        }

        public ActionResult LoginForChangedPassword(string username, string password)
        {
            try
            {
                string response = webportals.StaffLogin(username, password);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string staffNo = responseArr[1];
                        string staffName = responseArr[2];
                        Session["username"] = staffNo;
                        Session["staffName"] = staffName;

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

        public ActionResult LoginForUnchangedPassword(string username)
        {
            try
            {
                string response = webportals.LoginForUnchnagedPassword(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string staffNo = responseArr[1];
                        Session["staffNo"] = staffNo;
                        Session["staffEmail"] = responseArr[2];
                        return RedirectToAction("resetpassword", "login");
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
        public ActionResult ForgotPassword(User user)
        {
            try
            {
                string username = user.UserName;
                if (!ValidStaffNo(username))
                {
                    TempData["Error"] = "Ivalid staff No";
                    return RedirectToAction("forgotpassword", "login");
                }

                string password = GenerateOtp(14);
                string staffEmail = string.Empty;
                string response = webportals.GetStaffEmail(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    if (string.IsNullOrEmpty(responseArr[0]))
                    {
                        staffEmail = responseArr[1];
                    }
                    else
                    {
                        staffEmail = responseArr[0];
                    }
                }
                if (webportals.UpdateDefaultPassword(username, password))
                {
                    string subject = "OAG Portal Password Reset";
                    string body = $"Use below password to log into your portal." +
                        $"<br/><br/>" +
                        $"Portal password: <strong>{password}</strong>" +
                        $"<br/><br/>" +
                        $"<a href='{Request.Url.Scheme}://{Request.Url.Authority}'>Click here</a> to login in." +
                        $"<br/><br/>" +
                        $"Do not reply to this email.";
                    if (string.IsNullOrEmpty(staffEmail))
                    {
                        TempData["Error"] = "Please visit the HR office to update your email address!";
                        return RedirectToAction("index", "login");
                    }
                    Components.SentEmailAlerts(staffEmail, subject, body);
                    TempData["Success"] = $"Password reset code has been sent to your email address {staffEmail}";
                }
                return RedirectToAction("index", "login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("forgotpassword", "login");
            }
        }

        public ActionResult ResetPassword()
        {
            if (Session["staffNo"] == null) return RedirectToAction("index", "login");
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(User user)
        {
            try
            {
                string username = Session["staffNo"].ToString();
                string newPassword = user.NewPassword;

                if (!Components.ValidPassword(newPassword))
                {
                    TempData["Error"] = "Password must be at least 6 characters, no more than 20 characters, and must include at least one upper case letter, one lower case letter, one numeric digit and a special character.";
                    return RedirectToAction("resetpassword");
                }

                if (webportals.UpdateStaffPassword(username, newPassword))
                {
                    string staffEmail = Session["staffEmail"].ToString();
                    string subject = "Office of the Auditor General Portal Password Reset";
                    string body = $"Your portal password has been reset successfully. Use below password to login into your portal." +
                        $"<br/><br/>" +
                        $"Portal password: <strong>{newPassword}</strong>" +
                        $"<br/><br/>" +
                        $"Regards, Administrator.";
                    if (staffEmail != null) Components.SentEmailAlerts(staffEmail, subject, body);
                    TempData["Success"] = $"Password has been updated successfully. A copy of the password has been sent to your email address {staffEmail}.";
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    return RedirectToAction("index", "login");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("resetpassword", "login");
            }
            return View();
        }

        private bool ChangedPassword(string username)
        {
            bool changed = false;
            try
            {
                changed = webportals.CheckStaffPasswordChanged(username);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return changed;
        }

        private bool ValidStaffNo(string username)
        {
            bool valid = false;
            try
            {
                valid = webportals.CheckValidStaffNo(username);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return valid;
        }

        public static string GenerateOtp(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
    }
}