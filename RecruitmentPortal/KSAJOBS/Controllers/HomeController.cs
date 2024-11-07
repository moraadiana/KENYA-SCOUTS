using KSAJOBS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KSAJOBS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Get a list of advertised jobs
            //List<AdvertisedJobs> advertisedJobs = new List<AdvertisedJobs>();
            List<AdvertisedJobs> advertisedJobs = Services.GetAdvertisedJobs();

            // Pass the list to the view
            return View(advertisedJobs);
        }

        public ActionResult Details(string jobId, string refNo)
        {
            // Create an instance of job details
            JobDetail jobDetail = new JobDetail();

           // Get all job requirements
            var jobRequirements = Services.GetJobRequirements(jobId);

            //Get all job responsibilities
            var jobResponsibilities = Services.GetJobResponsibilities(jobId);

           // Get the job description
            string jobDescription = Services.GetJobDescription(jobId);
            ViewBag.JobTitle = jobDescription;

            /// Append the responsibilities and requirements
            jobDetail.JobResponsibilities = jobResponsibilities;
            jobDetail.JobRequirements = jobRequirements;

            // Pass the job details to the view
            return View(jobDetail);
        }

        [ChildActionOnly]
        public PartialViewResult Notification()
        {
            return PartialView("_Notification");
        }
    }
}