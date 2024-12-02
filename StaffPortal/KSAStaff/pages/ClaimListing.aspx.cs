using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Description;
using KSAStaff.NAVWS;

namespace KSAStaff.pages
{
    public partial class ClaimListing : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        Staffportal webportals = Components.ObjNav;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
            }
        }

        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {

                string username = Session["username"].ToString();
                string ClaimList = webportals.GetMyClaims(username);
                if (!string.IsNullOrEmpty(ClaimList))
                {
                    int counter = 0;
                    string[] ClaimListArr = ClaimList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string claimlist in ClaimListArr)
                    {
                        counter++;
                        string[] responseArr = claimlist.Split(strLimiters, StringSplitOptions.None);
                        if (responseArr.Length == 5)
                        {
                            var statusCls = "default";
                            string status = responseArr[4];
                            switch (status)
                            {
                                case "Pending":
                                    statusCls = "warning"; break;
                                case "1st Approval":
                                    statusCls = "default"; break;
                                case "2nd Approval":
                                    statusCls = "primary"; break;
                                case "Cheque Printing":
                                    statusCls = "success"; break;
                                case "Posted":
                                    statusCls = "success"; break;
                                case "Cancelled":
                                    statusCls = "danger"; break;
                                case "Checking":
                                    statusCls = "info"; break;
                                case "VoteBook":
                                    statusCls = "info"; break;
                                case "Pending Approval":
                                    statusCls = "primary"; break;
                                case "Approved":
                                    statusCls = "success"; break;
                            }



                            htmlStr += String.Format(@"
                            <tr  class='text-primary small'>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td >{3}</td>
                                <td style='width:500px;'>{4}</td>
                                <td><span class='label label-{5}'>{6}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='ClaimLines.aspx?ClaimNo={1}&query=old&status={6}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={2}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>
                        ",
                        counter,
                                responseArr[0],
                                responseArr[1],
                                responseArr[2],
                                responseArr[3],
                                statusCls,
                                responseArr[4]
                        );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return htmlStr;
        }
    }
}