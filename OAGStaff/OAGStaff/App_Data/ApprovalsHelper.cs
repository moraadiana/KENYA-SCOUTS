using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff
{
    public class ApprovalsHelper
    {
        private static Staffportal webportals = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };

        public static List<Approval> GetApprovals(string documentNo)
        {
            var list = new List<Approval>();
            try
            {
                string approvals = webportals.GetApprovalEntries(documentNo);
                if (!string.IsNullOrEmpty(approvals))
                {
                    string[] approvalsArr = approvals.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    int counter = 0;

                    foreach (string str in approvalsArr)
                    {
                        string[] responseArr = str.Split(strLimiters, StringSplitOptions.None);
                        counter++;
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
                            case "Canceled":
                                statusCls = "danger";
                                break;
                            case "Posted":
                                statusCls = "success";
                                break;
                            default:
                                statusCls = "info";
                                break;
                        }
                        Approval approval = new Approval()
                        {
                            Counter = counter,
                            EntryNo = Convert.ToInt32(responseArr[0]),
                            SequenceNo = Convert.ToInt32(responseArr[1]),
                            DateSentForApproval = Convert.ToDateTime(responseArr[2]),
                            SenderId = responseArr[3],
                            ApproverId = responseArr[4],
                            Comments = responseArr[5],
                            Status = responseArr[6],
                            StatusCls = statusCls
                        };
                        list.Add(approval);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }
    }
}