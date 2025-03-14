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
    public class DashboardController : Controller
    {
        Staffportal webportal = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            User user = new User();
            try
            {
                string path = Server.MapPath("~/Downloads/");
                if(!Directory.Exists(path)) Directory.CreateDirectory(path);
                string username = Session["username"].ToString();
                string staffName = Session["staffName"].ToString();
                string response = webportal.GetStaffDetails(username);
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
                    user.StaffNo = username;
                    user.StaffName = staffName;
                    user.JobId = jobId;
                    user.JobTitle = jobTitle;
                    user.Gender = gender;
                    user.PhoneNumber = phoneNumber;
                    user.IdNumber = idNumber;
                    user.EmailAddress = emailAddress;
                    user.PostalAddress = postalAddress;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("index", "login");
        }
    }
}