using OAGJobs.Models;
using OAGJobs.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGJobs.Controllers
{
    public class HomeController : Controller
    {
        Recruitment webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            Job job = new Job();
            try
            {
                //webportals.CloseAllRequisitions();
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

        public ActionResult Details(string jobId)
        {
            Job job = new Job();
            try
            {
                var requirements = new List<Job>();
                var responsibilities = new List<Job>();
                job.JobTitle = webportals.GetJobTitle(jobId);
                string jobrequirements = webportals.GetJobRequirements(jobId);
                if (!string.IsNullOrEmpty(jobrequirements))
                {
                    int counter = 0;
                    string[] jobRequirementsArr = jobrequirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in jobRequirementsArr)
                    {
                        counter++;
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Job requirement = new Job()
                        {
                            Counter = counter,
                            JobId = responseArr[0],
                            Description = responseArr[1],
                        };
                        requirements.Add(requirement);
                    }
                }

                string jobResponsibilities = webportals.GetJobResponsibilities(jobId);
                if (!string.IsNullOrEmpty(jobResponsibilities))
                {
                    string[] jobResponsibilitiesArr = jobResponsibilities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    int counter = 0;
                    foreach (string item in jobResponsibilitiesArr)
                    {
                        counter++;
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Job responsibility = new Job()
                        {
                            Counter = counter,
                            JobId = responseArr[0],
                            Description = responseArr[1],
                        };
                        responsibilities.Add(responsibility);
                    }
                }
                job.JobRequirements = requirements;
                job.JobResponsibilities = responsibilities;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return View(job);
        }
    }
}