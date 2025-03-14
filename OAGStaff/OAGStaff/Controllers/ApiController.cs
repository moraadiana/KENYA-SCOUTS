using OAGStaff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGStaff.Controllers
{
    public class ApiController : Controller
    {
        public JsonResult ValidateStartDate(string startDate)
        {
            string valid = HRHelper.ValidateLeaveStartDate(Convert.ToDateTime(startDate));
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LeaveBalance(string leaveType)
        {
            string username = Session["username"].ToString();
            string balance = HRHelper.LoadLeaveBalance(username, leaveType);
            return Json(balance, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalculateEndDate(string startDate, string appliedDays, string leaveType)
        {
            string username = Session["username"].ToString();
            HumanResource dates = HRHelper.CalculateDates(Convert.ToDateTime(startDate), Convert.ToInt32(appliedDays), leaveType);
            return Json(dates, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPeriodMonths(string periodYear)
        {
            var periodMonths = HRHelper.GetPayrolMonths(Convert.ToInt32(periodYear));
            return Json(periodMonths, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsVoteBudgeted(string period, string vote, string amount)
        {
            var budgeted = ApiHelper.IsVoteBudgeted(period.Replace("%", " "), vote, Convert.ToDecimal(amount));
            return Json(budgeted, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyCurrentPassword(string username, string password)
        {
            var verify = Helper.VerifyCurrentPassword(username,password);
            return Json(verify, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsStudyLeave(string leaveType)
        {
            string str = HRHelper.StudyLeave(leaveType);
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEvaluationScores(string requisitionNo, string applicationNo)
        {
            var str = ApiHelper.GetEvaluationScores(requisitionNo,applicationNo);
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveDetails(string leaveNo)
        {
            var str = ApiHelper.GetLeaveDetails(leaveNo);
            return Json(str, JsonRequestBehavior.AllowGet);
        }
    }
}