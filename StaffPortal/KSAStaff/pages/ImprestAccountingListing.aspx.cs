using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Description;

namespace KSAStaff.pages
{
    public partial class ImprestAccountingListing : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
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

        public string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetMyImprestSurrender",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@userID", "'" + username + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var statusCls = "default";
                        string status = reader["MyStatus"].ToString();
                        
                        switch (status)
                        {
                            case "Pending":
                                statusCls = "warning"; break;
                            case "1st Approval":
                                statusCls = "primary"; break;
                           
                            case "2nd Approval":
                                statusCls = "primary"; break;
                            case "Cheque Printing":
                                statusCls = "success"; break;
                            case "Posted":
                                statusCls = "info"; break;
                            case "Checking":
                                statusCls = "success"; break;
                            case "VoteBook":
                                statusCls = "success"; break;
                            case "Pending Approval":
                                statusCls = "success"; break;
                            case "Approved":
                                statusCls = "success"; break;
                            case "Archive":
                                statusCls = "danger"; break;
                            
                        }

                        htmlStr += string.Format(@"
                                    <tr  class='text-primary small'>
                                        <td>{0}</td>
                                        <td>{1}</td>
                                        <td>{2}</td>
                                        <td><span class='label label-{4}'>{3}</span></td>
                                        <td>
                                            <div class='options btn-group' >
					                            <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                            <ul class='dropdown-menu'>
                                                    <li><a href='ImprestAccountingLines.aspx?SurrenderNo={0}&ImprestNo={1}&query=old&status={3}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Details</span></a></li>
                                                    <li><a href='ApprovalTracking.aspx?DocNum={0}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                                </ul>	
                                            </div>
                                        </td>
                                    </tr>",
                                reader["No"],
                                reader["Imprest Issue Doc_ No"],
                                reader["Payee"],
                                status,
                                statusCls
                        );
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