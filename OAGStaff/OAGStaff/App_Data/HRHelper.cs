using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OAGStaff
{
    public class HRHelper
    {
        private static Staffportal webportal = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };
        private static SqlConnection connection;
        private static SqlCommand command;
        private static SqlDataReader reader;

        public static List<HumanResource> GetLeaveApplications(string username)
        {
            var list = new List<HumanResource>();
            try
            {
                string leaves = webportal.GetStaffLeaveRequests(username);
                if (!string.IsNullOrEmpty(leaves))
                {
                    string[] leavesArr = leaves.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    Array.Reverse(leavesArr);
                    int counter = 0;
                    foreach (string leave in leavesArr)
                    {
                        string[] responseArr = leave.Split(strLimiters, StringSplitOptions.None);
                        counter++;
                        string statusCls = "default";
                        string status = responseArr[7];
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
                        HumanResource lv = new HumanResource()
                        {
                            Counter = counter,
                            LeaveNo = responseArr[0],
                            ApplicationDate = responseArr[1],
                            LeaveType = responseArr[2],
                            AppliedDays = responseArr[3],
                            StartDate = responseArr[4],
                            EndDate = responseArr[5],
                            ReturnDate = responseArr[6],
                            Status = responseArr[7],
                            StatusCls = statusCls,
                            Course = responseArr[8],
                            PeriodOfStudy = responseArr[9],
                            UniversityOfStudy = responseArr[10],
                            CountryOfStudy = responseArr[11]
                        };
                        list.Add(lv);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetLeaveTypes(string username)
        {
            var list = new List<HumanResource>();
            try
            {
                string gender = webportal.GetStaffGender(username);
                string leaveTypes = webportal.GetStaffLeaveTypes(gender);
                //string leaveTypes = webportal.GetLeaveTypes();
                if (!string.IsNullOrEmpty(leaveTypes))
                {
                    string[] leaveTypesArr = leaveTypes.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string leaveType in leaveTypesArr)
                    {
                        string[] responseArr = leaveType.Split(strLimiters, StringSplitOptions.None);
                        if (webportal.IsStudyLeave(responseArr[0])) continue;
                        HumanResource leave = new HumanResource()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                        };
                        list.Add(leave);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetStudyLeaves()
        {
            var list = new List<HumanResource>();
            try
            {
                string leaveTypes = webportal.GetStudyLeaves();
                if (!string.IsNullOrEmpty(leaveTypes))
                {
                    string[] leaveTypesArr = leaveTypes.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string leaveType in leaveTypesArr)
                    {
                        string[] responseArr = leaveType.Split(strLimiters, StringSplitOptions.None);
                        HumanResource leave = new HumanResource()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                        };
                        list.Add(leave);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static string ValidateLeaveStartDate(DateTime startDate)
        {
            string valid = string.Empty;
            try
            {
                valid = webportal.ValidateStartDate(startDate);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return valid;
        }

        public static HumanResource CalculateDates(DateTime startDate, int appliedDays, string leaveType)
        {
            HumanResource dates = new HumanResource();
            try
            {
                string endDate = webportal.CalcEndDate(startDate, appliedDays, leaveType).ToString();
                string returnDate = webportal.CalcReturnDate(Convert.ToDateTime(endDate), leaveType).ToString();
                dates.EndDate = endDate;
                dates.ReturnDate = returnDate;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return dates;
        }

        public static string LoadLeaveBalance(string username, string leaveType)
        {
            string balance = string.Empty;
            try
            {
                if (leaveType == "ANNUAL") balance = webportal.AvailableLeaveDays(username, leaveType);
                else balance = webportal.DefaultDays(leaveType);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return balance == "0" ? "Not Available" : balance;
        }

        public static string[] GetRelieverDetails(string relieverNo)
        {
            string[] reliever = new string[4];
            try
            {
                string response = webportal.GetLeaveRelieverDetails(relieverNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    reliever[0] = responseArr[0];
                    reliever[1] = responseArr[1];
                    reliever[2] = responseArr[2];
                    reliever[3] = responseArr[3];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return reliever;
        }

        public static List<HumanResource> GetPeriodYears()
        {
            var list = new List<HumanResource>();
            try
            {
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetPayslipYears",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue(@"Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        HumanResource periodYear = new HumanResource()
                        {
                            PeriodYear = reader["Period Year"].ToString()
                        };
                        list.Add(periodYear);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetPayrolMonths(int periodYear)
        {
            var list = new List<HumanResource>();
            try
            {
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetPayslipMonths",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue(@"Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@current", "'" + periodYear + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        HumanResource periodMonth = new HumanResource()
                        {
                            PeriodMonth = reader["Period Month"].ToString(),
                            PeriodName = reader["Period Name"].ToString()
                        };
                        list.Add(periodMonth);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetPnineYears(string username)
        {
            var list = new List<HumanResource>();
            try
            {
                string pnineYears = webportal.GetPnineYears(username);
                if (!string.IsNullOrEmpty(pnineYears))
                {
                    string[] pnineYearsArr = pnineYears.Split(strLimiters,StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                    HumanResource p9Year = new HumanResource()
                    {
                        PeriodYear = pnineYearsArr[0],
                    };
                    list.Add(p9Year);
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetStaffBtos(string username)
        {
            var list = new List<HumanResource>();
            try
            {
                string staffBtos = webportal.GetStaffLeaveBto(username);
                if (!string.IsNullOrEmpty(staffBtos))
                {
                    string[] staffBtosArr = staffBtos.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string bto in staffBtosArr)
                    {
                        string[] responseArr = bto.Split(strLimiters,StringSplitOptions.None);
                        HumanResource human = new HumanResource()
                        {
                            DocumentNo = responseArr[0],
                            LeaveType = responseArr[1],
                            AppliedDays = responseArr[2],
                            StartDate = responseArr[3],
                            EndDate = responseArr[4],
                            ReturnDate = responseArr[5],
                            Status = responseArr[6]
                        };
                        list.Add(human);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetIncidentTypes()
        {
            var list = new List<HumanResource>();
            try
            {
                string staffBtos = webportal.GetIncidentTypes();
                if (!string.IsNullOrEmpty(staffBtos))
                {
                    string[] staffBtosArr = staffBtos.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string bto in staffBtosArr)
                    {
                        string[] responseArr = bto.Split(strLimiters, StringSplitOptions.None);
                        HumanResource human = new HumanResource()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                        };
                        list.Add(human);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetPostedLeaves(string username)
        {
            var list = new List<HumanResource>();
            try
            {
                string leaves = webportal.GetPostedLeaves(username);
                if (!string.IsNullOrEmpty(leaves))
                {
                    string[] leavesArr = leaves.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string bto in leavesArr)
                    {                       
                        HumanResource human = new HumanResource()
                        {
                            LeaveNo = bto,
                        };
                        list.Add(human);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<HumanResource> GetLeavesToRecall()
        {
            var list = new List<HumanResource>();
            try
            {
                string leaves = webportal.GetLeaveToRecall();
                if (!string.IsNullOrEmpty(leaves))
                {
                    string[] leavesArr = leaves.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string leave in leavesArr)
                    {
                        string[] responseArr = leave.Split(strLimiters, StringSplitOptions.None);
                        if (webportal.IsLeaveRecalled(responseArr[0])) continue;
                        list.Add(new HumanResource()
                        {
                            LeaveNo = responseArr[0],
                            Description = $"{responseArr[0]} => {responseArr[1]} => {responseArr[2]}"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }        

        public static string StudyLeave(string leaveType)
        {
            return webportal.IsStudyLeave(leaveType).ToString();
        }
    }
}