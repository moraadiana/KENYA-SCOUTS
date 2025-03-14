using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGStaff.Controllers
{
    public class AppraisalController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult AppraisalListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Appraisal resource = new Appraisal();
            try
            {
                string username = Session["username"].ToString();
                var appraisals = AppraisalHelper.GetMyAppraisals(username);
                resource.Appraisals = appraisals;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appraisallisting", "appraisal");
            }
            return View(resource);
        }

        public ActionResult AppraisalHeader()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Appraisal resource = new Appraisal();
            try
            {
                string username = Session["username"].ToString();
                string staffName = Session["staffName"].ToString();
                var staffDetails = Helper.GetEmployeeDepartmentDetails(username);
                var employees = Helper.GetAllEmployees();
                resource.Directorate = staffDetails.Directorate;
                resource.Department = staffDetails.Department;
                resource.StaffName = staffName;
                resource.StaffNo = username;
                resource.Employees = employees;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appraisalheader", "appraisal");
            }
            return View(resource);
        }

        [HttpPost]
        public ActionResult AppraisalHeader(Appraisal appraisal)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            try
            {
                string username = Session["username"].ToString();
                string supervisor = appraisal.Supervisor;
                string resposne = webportals.CreateAppraisalHeader(username, supervisor);
                if (!string.IsNullOrEmpty(resposne))
                {
                    string[] responseArr = resposne.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string appraisalNo = responseArr[1];
                        return RedirectToAction("appraisallines", "appraisal", new { appraisalNo, status = "New" });
                    }
                    else if (returnMsg == "FAILED")
                    {
                        TempData["Error"] = "An error occured while creating appraisal header. Please try again later!";
                        return RedirectToAction("appraisalheader", "appraisal");
                    }
                    else
                    {
                        TempData["Error"] = returnMsg;
                        return RedirectToAction("appraisalheader", "appraisal");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appraisalheader", "appraisal");
            }
            return View();
        }

        public ActionResult AppraisalLines(string appraisalNo, string status)
        {
            if (appraisalNo == null) return RedirectToAction("appraisalheader", "appraisal");
            Appraisal appraisal = new Appraisal();
            try
            {
                string midYearReviews = webportals.GetMidYearAppraisalDates();
                if (!string.IsNullOrEmpty(midYearReviews))
                {
                    string[] midYearReviewArr = midYearReviews.Split(strLimiters, StringSplitOptions.None);
                    DateTime midYearStartDate = Convert.ToDateTime(midYearReviewArr[0]);
                    DateTime midYearEndDate = Convert.ToDateTime(midYearReviewArr[1]);

                    if (midYearStartDate >= DateTime.Today && DateTime.Today <= midYearEndDate) appraisal.MidYearReview = true;
                    else appraisal.MidYearReview = false;
                }
                webportals.GetIndividualSubActivities(appraisalNo);
                var subLines = AppraisalHelper.GetAppraisalSubLines(appraisalNo, status);
                var appraisalValues = AppraisalHelper.GetAppraisalsValues(appraisalNo);
                appraisal.AppraisalsSubLines = subLines;
                appraisal.AppraisalsValues = appraisalValues;
                appraisal.Status = status;
                appraisal.AppraisalNo = appraisalNo;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appraisallines", "appraisal", new { appraisalNo, status });
            }
            return View(appraisal);
        }

        public ActionResult SentAppraisalApproval(string appraisalNo, string status)
        {
            try
            {
                string approval = webportals.OnSendAppraisalForApproval(appraisalNo);
                if (!string.IsNullOrEmpty(approval))
                {
                    if (approval == "SUCCESS")
                    {
                        TempData["Success"] = $"Appraisal number {appraisalNo} has been sent for approval successfully";
                        return RedirectToAction("appraisallisting", "appraisal");
                    }
                    else
                    {
                        TempData["Error"] = approval;
                        return RedirectToAction("appraisallines", "appraisal", new { appraisalNo, status });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appraisallines", "appraisal", new { appraisalNo, status });
            }
            return View();
        }

        public ActionResult CancelAppraisal(string appraisalNo, string status)
        {
            try
            {
                webportals.OnCancelAppraisal(appraisalNo);
                TempData["Success"] = $"Document {appraisalNo} has been cancelled successfully!";
                return RedirectToAction("appraisallisting", "appraisal");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("appraisallines", "appraisal", new { appraisalNo, status });
            }
        }

        public ActionResult InitiateMidYearReview(string appraisalNo)
        {
            return View();
        }

        public ActionResult MidYearAppraisalListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Appraisal resource = new Appraisal();
            try
            {
                string username = Session["username"].ToString();
                var appraisals = AppraisalHelper.GetMyMidYearAppraisals(username);
                resource.Appraisals = appraisals;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("midyearappraisallisting", "appraisal");
            }
            return View(resource);
        }

        public ActionResult EndYearAppraisalListing()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Appraisal resource = new Appraisal();
            try
            {
                string username = Session["username"].ToString();
                var appraisals = AppraisalHelper.GetMyEndYearAppraisals(username);
                resource.Appraisals = appraisals;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("midyearappraisallisting", "appraisal");
            }
            return View(resource);
        }

        public ActionResult MidYearAppraisalLines(string appraisalNo, string status)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Appraisal appraisal = new Appraisal();
            try
            {
                string midYearReviews = webportals.GetMidYearAppraisalDates();
                if (!string.IsNullOrEmpty(midYearReviews))
                {
                    string[] midYearReviewArr = midYearReviews.Split(strLimiters, StringSplitOptions.None);
                    DateTime midYearStartDate = Convert.ToDateTime(midYearReviewArr[0]);
                    DateTime midYearEndDate = Convert.ToDateTime(midYearReviewArr[1]);

                    if (midYearStartDate >= DateTime.Today && DateTime.Today <= midYearEndDate) appraisal.MidYearReview = true;
                    else appraisal.MidYearReview = false;
                }
                webportals.GetIndividualSubActivities(appraisalNo);
                var subLines = AppraisalHelper.GetAppraisalSubLines(appraisalNo, status);
                var appraisalValues = AppraisalHelper.GetAppraisalsValues(appraisalNo);
                var assessmentScores = AppraisalHelper.GetAssessmentScores();
                appraisal.AppraisalsSubLines = subLines;
                appraisal.AppraisalsValues = appraisalValues;
                appraisal.Status = status;
                appraisal.AppraisalNo = appraisalNo;
                appraisal.AssessmentScores = assessmentScores;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(appraisal);
        }

        [HttpPost]
        public ActionResult MidYearAppraisalLines(Appraisal appraisal)
        {
                string appraisalNo = appraisal.AppraisalNo;
            string status = appraisal.Status;
            try
            {
                string categories = appraisal.SelectedCategories;
                string scoreCategories = appraisal.SelfAssessmentCategories;
                string[] categoriesArr = categories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                string[] scoreCategoriesArr = scoreCategories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);

                foreach (string category in categoriesArr)
                {
                    string[] responseArr = category.Split(strLimiters, StringSplitOptions.None);
                    decimal rating = Convert.ToInt32(responseArr[0]);
                    string description = responseArr[1];

                    foreach (string score in scoreCategoriesArr)
                    {
                        string[] scoreArr = score.Split(strLimiters, StringSplitOptions.None);
                        decimal selfScore = Convert.ToInt32(scoreArr[0]);
                        if (scoreArr[1] == description) webportals.InsertAppraisalValues(appraisalNo, description, rating, selfScore);
                    }
                }
                webportals.OnSendAppraisalForApproval(appraisalNo);
                TempData["Success"] = "Approval request has been submitted successfully!";
                return RedirectToAction("midyearappraisallisting","appraisal");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("midyearappraisallines", "appraisal", new {appraisalNo,status});
            }
        }

        public JsonResult GetAppraisalValues(string appraisalNo, string description)
        {
            var items = AppraisalHelper.GetAppraisalValues(appraisalNo, description);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}