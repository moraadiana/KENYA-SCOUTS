using KSAJOBS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace KSAJOBS.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("Index", "home");
            // Get a list of advertised jobs
            List<AdvertisedJobs> advertisedJobs = Services.GetAdvertisedJobs();

            // Pass the list to the view
            return View(advertisedJobs);
        }

        public ActionResult Applications()
        {
            if (Session["username"] == null) return RedirectToAction("index", "home");

            // Get the username
            string username = Session["username"].ToString();

            // Get the list of the applicants applications
            List<MyApplication> myApplications = Services.GetMyApplications(username);

            // Append list to the view
            return View(myApplications);
        }

        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            return PartialView("_Notification");
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("index", "home");
        }
    }
}