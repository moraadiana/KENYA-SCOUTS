using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using OAGStaff.Models;
using OAGStaff.NAVWS;

namespace OAGStaff.Controllers
{
    public class RecruitmentController : Controller
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult CommitteeLogin()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Recruitment recruitment = new Recruitment();
            try
            {
                var requisitions = RecruitmentHelper.GetRequisitions();
                recruitment.Requisitions = requisitions;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(recruitment);
        }

        [HttpPost]
        public ActionResult CommitteeLogin(Recruitment recruitment)
        {
            try
            {
                string username = Session["username"].ToString();
                string requisitionNo = recruitment.RequisitionNo;
                string password = recruitment.Password;
                string documentNo = webportals.GetCommitteNo(requisitionNo);
                if (!webportals.HasBeenAppointed(requisitionNo, username))
                {
                    TempData["Error"] = $"You have not been appointed to this {documentNo} committee!";
                    return RedirectToAction("committeelogin", "recruitment");
                }
                string response = webportals.CommitteeMemberLogin(requisitionNo, username, password);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string committeeNo = responseArr[0];
                    string staffNo = responseArr[1];
                    string staffName = responseArr[2];
                    string role = responseArr[3];
                    return RedirectToAction("recruitmentevaluation", "recruitment", new { requisitionNo, role });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("committeelogin", "recruitment");
            }
            return View();
        }

        public ActionResult RecruitmentEvaluation(string requisitionNo, string role)
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            Recruitment recruitment = new Recruitment();
            try
            {
                string jobTitle = webportals.GetJobTitle(requisitionNo);
                var applicants = RecruitmentHelper.GetApplicants(requisitionNo);
                recruitment.JobApplicants = applicants;
                recruitment.JobTitle = jobTitle;
                recruitment.RequisitionNo = requisitionNo;
                recruitment.Role = role;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(recruitment);
        }

        [HttpPost]
        public ActionResult RecruitmentEvaluation(Recruitment recruitment)
        {
            string requisitionNo = recruitment.RequisitionNo;
            string role = recruitment.Role;
            try
            {
                string categories = recruitment.SelectedCategories;
                string[] categoriesArr = categories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                foreach (string category in categoriesArr)
                {
                    string[] responseArr = category.Split(strLimiters, StringSplitOptions.None);
                    int shortlist = Convert.ToInt32(responseArr[0]);
                    string remarks = responseArr[1];
                    string applicationNo = responseArr[3];
                    string comments = recruitment.Comments;
                    webportals.InsertEvaluationScores(requisitionNo, applicationNo, shortlist, remarks, comments);
                }
                TempData["Success"] = "You have successfully submitted the recruitment evaluation!";
                return RedirectToAction("recruitmentevaluation", "recruitment", new { requisitionNo, role });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("recruitmentevaluation", "recruitment", new { requisitionNo, role });
            }
        }

        [HttpPost]
        public ActionResult AcceptEvaluation(Recruitment recruitment)
        {
            string requisitionNo = recruitment.RequisitionNo;
            string role = recruitment.Role;
            try
            {
                string username = Session["username"].ToString();
                string remarks = recruitment.Comments;
                webportals.AcceptEvaluationDecision(requisitionNo, username, remarks);
                TempData["Success"] = "You have successfully accepted the decission!";
                return RedirectToAction("recruitmentevaluation", "recruitment", new { requisitionNo, role });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("recruitmentevaluation", "recruitment", new { requisitionNo, role });
            }
        }
    }
}