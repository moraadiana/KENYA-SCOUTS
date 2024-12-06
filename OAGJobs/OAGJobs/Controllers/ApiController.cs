using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGJobs.Controllers
{
    public class ApiController : Controller
    {
        public JsonResult QualificationCodes(string qualificationType)
        {
            string jobId = Session["jobId"].ToString();
            var qualificationCodes = Services.GetQualificationCodes(jobId,qualificationType);
            return Json(qualificationCodes,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreasOfSpecialization(string areaOfStudy)
        {
            var specializations = Services.GetAreasOfSpecialization(areaOfStudy);
            return Json(specializations,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAwards(string areaOfSpecialization)
        {
            var specializations = Services.GetAwards(areaOfSpecialization);
            return Json(specializations, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourses(string award, string areaOfSpecialization)
        {
            var specializations = Services.GetCourses(award, areaOfSpecialization);
            return Json(specializations, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGrades(string course)
        {
            var specializations = Services.GetGrades(course);
            return Json(specializations, JsonRequestBehavior.AllowGet);
        }
    }
}