using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGStaff.Controllers
{
    public class SettingsController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        public ActionResult ChangePassword()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(Setting setting)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            try
            {
                string username = Session["username"].ToString();
                string newPassword = setting.Newpassword;
                string oldPassword = setting.OldPassword;

                if (!webportals.ValidateOldPassword(username, oldPassword))
                {
                    TempData["Error"] = "Old password is invalid";
                    return RedirectToAction("changepassword");
                }
                if (webportals.UpdateStaffPassword(username, newPassword))
                {
                    TempData["Success"] = "Password has been updated successfully";
                    return RedirectToAction("index", "dashboard");
                }
                else
                {
                    TempData["Error"] = "An erro occured while updating password. Please try again later.";
                    return RedirectToAction("changepassword", "settings");
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("changepassword", "settings");
            }
        }
    }
}