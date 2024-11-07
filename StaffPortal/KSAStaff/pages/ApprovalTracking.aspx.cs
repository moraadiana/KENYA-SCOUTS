using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Messaging;

namespace KSAStaff.pages
{
    public partial class ApprovalTracking : System.Web.UI.Page
    {
        SqlDataReader reader;
        SqlCommand command;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                ApprovalTracks();
            }
        }

        protected string ApprovalTracks()
        {
            var htmlStr = string.Empty;
            try
            {
                using (var conn = Components.getconnToNAV())
                {
                    var cmd = new SqlCommand();
                    cmd.CommandText = "spGetMyApprovalTracks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    cmd.Parameters.AddWithValue("@DocumentNo", "'" + Request.QueryString["DocNum"].ToString() + "'");

                    using (var reader = cmd.ExecuteReader())
                    {
                        int counter = 0;
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                counter++;
                                var statusCls = "default";
                                string status = reader["MyStatus"].ToString();
                                switch (status)
                                {
                                    case "Created":

                                    case "Open":
                                        statusCls = "warning"; break;
                                    case "Cancelled":
                                        statusCls = "primary"; break;
                                    case "Reject":
                                    case "Approved":
                                        statusCls = "success"; break;
                                    case "":
                                        statusCls = "info"; break;
                                }

                                htmlStr += string.Format(
                                    @"<tr class='text-info small'>
                            <td>{0}</td>
                            <td>{1}</td>
                            <td>{2}</td>
                            <td>{3}</td>
                            <td>{4}</td>
                            <td>{5}</td>
                            <td>{6}</td>
                            </tr>",
                                    counter,
                                    reader["Entry No_"],
                                    reader["Sequence No_"],
                                    Convert.ToDateTime(reader["Date-Time Sent for Approval"]),
                                    reader["Sender ID"],
                                    reader["Approver ID"],
                                   status,
                                   statusCls
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                htmlStr = $"Error: {ex.Message}";
            }

            return htmlStr;
        }

    }
}