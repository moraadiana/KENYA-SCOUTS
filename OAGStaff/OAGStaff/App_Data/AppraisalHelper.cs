using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff
{
    public class AppraisalHelper
    {
        private static Staffportal webportal = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };
        public static List<Appraisal> GetMyAppraisals(string username)
        {
            var list = new List<Appraisal>();
            try
            {
                string appraisals = webportal.GetMyAppraisals(username);
                if (!string.IsNullOrEmpty(appraisals))
                {
                    string[] appraisalsArr = appraisals.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in appraisalsArr)
                    {
                        string[] responseArr = str.Split(strLimiters, StringSplitOptions.None);
                        string statusCls = "default";
                        string status = responseArr[5];
                        switch (status)
                        {
                            case "Open":
                                statusCls = "warning";
                                break;
                            case "Pending Approval":
                                statusCls = "primary";
                                break;
                            case "Approved":
                                statusCls = "success";
                                break;
                            case "Cancelled":
                                statusCls = "danger";
                                break;
                            case "Posted":
                                statusCls = "success";
                                break;
                            default:
                                statusCls = "info";
                                break;
                        }
                        Appraisal appraisal = new Appraisal()
                        {
                            AppraisalNo = responseArr[0],
                            Date = responseArr[1],
                            Period = responseArr[2],
                            StaffNo = responseArr[3],
                            StaffName = responseArr[4],
                            Status = responseArr[5],
                            StatusCls = statusCls,
                        };
                        list.Add(appraisal);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Appraisal> GetMyMidYearAppraisals(string username)
        {
            var list = new List<Appraisal>();
            try
            {
                string appraisals = webportal.GetMyMidYearAppraisals(username);
                if (!string.IsNullOrEmpty(appraisals))
                {
                    string[] appraisalsArr = appraisals.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in appraisalsArr)
                    {
                        string[] responseArr = str.Split(strLimiters, StringSplitOptions.None);
                        string statusCls = "default";
                        string status = responseArr[5];
                        switch (status)
                        {
                            case "Open":
                                statusCls = "warning";
                                break;
                            case "Pending Approval":
                                statusCls = "primary";
                                break;
                            case "Approved":
                                statusCls = "success";
                                break;
                            case "Cancelled":
                                statusCls = "danger";
                                break;
                            case "Posted":
                                statusCls = "success";
                                break;
                            default:
                                statusCls = "info";
                                break;
                        }
                        Appraisal appraisal = new Appraisal()
                        {
                            AppraisalNo = responseArr[0],
                            Date = responseArr[1],
                            Period = responseArr[2],
                            StaffNo = responseArr[3],
                            StaffName = responseArr[4],
                            Status = responseArr[5],
                            StatusCls = statusCls,
                        };
                        list.Add(appraisal);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Appraisal> GetMyEndYearAppraisals(string username)
        {
            var list = new List<Appraisal>();
            try
            {
                string appraisals = webportal.GetMyEndYearAppraisals(username);
                if (!string.IsNullOrEmpty(appraisals))
                {
                    string[] appraisalsArr = appraisals.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in appraisalsArr)
                    {
                        string[] responseArr = str.Split(strLimiters, StringSplitOptions.None);
                        string statusCls = "default";
                        string status = responseArr[5];
                        switch (status)
                        {
                            case "Open":
                                statusCls = "warning";
                                break;
                            case "Pending Approval":
                                statusCls = "primary";
                                break;
                            case "Approved":
                                statusCls = "success";
                                break;
                            case "Cancelled":
                                statusCls = "danger";
                                break;
                            case "Posted":
                                statusCls = "success";
                                break;
                            default:
                                statusCls = "info";
                                break;
                        }
                        Appraisal appraisal = new Appraisal()
                        {
                            AppraisalNo = responseArr[0],
                            Date = responseArr[1],
                            Period = responseArr[2],
                            StaffNo = responseArr[3],
                            StaffName = responseArr[4],
                            Status = responseArr[5],
                            StatusCls = statusCls,
                        };
                        list.Add(appraisal);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Appraisal> GetAppraisalSubLines(string appraisalNo, string status)
        {
            var list = new List<Appraisal>();
            try
            {
                string subLines = webportal.GetAppraisalsSubLines(appraisalNo);
                if (!string.IsNullOrEmpty(subLines))
                {
                    string[] subLinesArr = subLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string line in subLinesArr)
                    {
                        string[] responseArr = line.Split(strLimiters, StringSplitOptions.None);
                        Appraisal appraisal = new Appraisal()
                        {
                            SubCode = responseArr[0],
                            SubActivity = responseArr[1],
                            PerformanceCriteria= responseArr[2],
                            UnitOfMeasure= responseArr[3],
                            Target= responseArr[4],
                            Status = status
                        };
                        list.Add(appraisal);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Appraisal> GetAppraisalsValues(string appraisalNo)
        {
            var list = new List<Appraisal>();
            try
            {
                string subLines = webportal.GetAppraisalsValues(appraisalNo);
                if (!string.IsNullOrEmpty(subLines))
                {
                    string[] subLinesArr = subLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in subLinesArr)
                    {
                        string[] responseArr = line.Split(strLimiters, StringSplitOptions.None);
                        Appraisal appraisal = new Appraisal()
                        {
                            SubCode = responseArr[0],
                            Description = responseArr[1],
                            UnitOfMeasure = responseArr[2],
                            Target = responseArr[3],
                            SelfAssessment = responseArr[4],
                            SupervisorAssessment = responseArr[5],
                            AgreedRating = responseArr[6],
                        };
                        list.Add(appraisal);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Appraisal> GetAssessmentScores()
        {
            var list = new List<Appraisal>();
            try
            {
                string subLines = webportal.GetSelfAssessmentScores();
                if (!string.IsNullOrEmpty(subLines))
                {
                    string[] subLinesArr = subLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in subLinesArr)
                    {
                        string[] responseArr = line.Split(strLimiters, StringSplitOptions.None);
                        Appraisal appraisal = new Appraisal()
                        {
                            Rating = Convert.ToInt32(responseArr[0]),
                            Description = responseArr[1],
                        };
                        list.Add(appraisal);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static Appraisal GetAppraisalValues(string appraisalNo, string description)
        {
            Appraisal appraisal = new Appraisal();
            try
            {
                string appraisalValues = webportal.GetAppraisalValues(appraisalNo,description);
                if (!string.IsNullOrEmpty(appraisalValues))
                {
                    string[] appraisalValuesArr = appraisalValues.Split(strLimiters, StringSplitOptions.None);
                    appraisal.Rating = Convert.ToInt32(appraisalValuesArr[0]);
                    appraisal.Description = appraisalValuesArr[1];

                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return appraisal;
        }
    }
}