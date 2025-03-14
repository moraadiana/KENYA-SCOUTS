using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OAGStaff
{
    public class ApiHelper
    {
        private static Staffportal webportals = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };

        public static string IsVoteBudgeted(string period, string vote, decimal amount)
        {
            string budgeted = string.Empty;
            try
            {
                budgeted = webportals.IsVoteBudgeted(period, vote, amount);
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return budgeted;
        }

        public static Recruitment GetEvaluationScores(string requisitionNo, string applicationNo)
        {
            Recruitment recruitment = new Recruitment();
            try
            {
                string response = webportals.GetEvaluationScores(requisitionNo, applicationNo);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters,StringSplitOptions.None);
                    recruitment.Shortlisted = responseArr[0] == "Yes" ? 2 : 1;
                    recruitment.Remarks = responseArr[1];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return recruitment;
        }

        public static HumanResource GetLeaveDetails(string leaveNo)
        {
            HumanResource resource = new HumanResource();
            try
            {
                string leaveDetails = Components.ObjNav.GetLeaveDetails(leaveNo);
                if (!string.IsNullOrEmpty(leaveDetails))
                {
                    string[] leaveDetailsArr = leaveDetails.Split(strLimiters, StringSplitOptions.None);
                    resource.StartDate = Convert.ToDateTime(leaveDetailsArr[4]).ToString("dd MMM yyyy");
                    resource.EndDate = Convert.ToDateTime(leaveDetailsArr[5]).ToString("dd MMM yyyy");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return resource;
        }
    }
}