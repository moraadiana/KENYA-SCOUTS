using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using KSAStaff.NAVWS;

namespace KSAStaff.pages
{
    public partial class LeaveListing : System.Web.UI.Page
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

                if (Request.QueryString["leaveNo"] != null)
                {
                    string leaveNo = Request.QueryString["leaveNo"].ToString();
                    Components.ObjNav.OnCancelLeaveApplication(leaveNo);
                    Response.Redirect("LeaveListing.aspx");
                }
            }
        }

        /*  protected string Jobs()
          {
              var htmlStr = string.Empty;
              try
              {
                  string username = Session["username"].ToString();
                  connection = Components.getconnToNAV();
                  command = new SqlCommand()
                  {
                      CommandText = "spGetMyLeaveApps",
                      CommandType = CommandType.StoredProcedure,
                      Connection = connection
                  };
                  command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                  command.Parameters.AddWithValue("@Employee_No", "'" + username + "'");
                  reader = command.ExecuteReader();
                  if (reader.HasRows)
                  {
                      int counter = 0;
                      while (reader.Read())
                      {
                          counter++;
                         var statusCls = "default";
                          string status = reader["Status"].ToString();

                          switch (status)
                          {
                              case "Open":
                                  statusCls = "warning";
                                  break;
                              case "Released":
                                  statusCls = "primary";
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
                          }
                          htmlStr += String.Format(@"
                              <tr>
                                  <td>{0}</td>
                                  <td>{1}</td>
                                  <td>{2}</td>
                                  <td>{3}</td>
                                  <td>{4}</td>
                                  <td>{5}</td>
                                  <td>{6}</td>
                                  <td>{7}</td>
                                  <td><span class='label label-{9}'>{8}</span></td>
                                  <td class='small'>
                                      <div class='options btn-group' >
                                          <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
                                          <ul class='dropdown-menu'>
                                              <li><a href='LeaveListing.aspx?leaveNo={1}&status={8}'><i class='fa fa-trash text-danger'></i><span class='text-danger'>Cancel Approval Request</span></a></li>
                                              <li><a href='ApprovalTracking.aspx?DocNum={1}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                          </ul>	
                                      </div>
                                  </td>
                              </tr>
                              ",
                              counter,
                              reader["No_"].ToString(),
                              reader["Leave Type"].ToString(),
                              Convert.ToInt32(Convert.ToDouble(reader["Applied Days"])),
                              Convert.ToDateTime(reader["$systemCreatedAt"]).ToShortDateString(),
                              Convert.ToDateTime(reader["Starting Date"]).ToShortDateString(),
                              Convert.ToDateTime(reader["end Date"]).ToShortDateString(),
                              Convert.ToDateTime(reader["Return Date"]).ToShortDateString(),
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
          }*/
        protected string Jobs()
        {
            var htmlStr = string.Empty;
            try
            {
                string username = Session["username"].ToString();
                string leaveList = webportals.GetMyleaveApplications(username);
                if (!string.IsNullOrEmpty(leaveList))
                {
                    int counter = 0;
                    string[] leaveListArr = leaveList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string leavelist in leaveListArr)
                    {
                        counter++;
                        string[] responseArr = leavelist.Split(strLimiters, StringSplitOptions.None);
                        var statusCls = "default";
                        string status = responseArr[7];
                        switch (status)
                        {
                            case "Open":
                                statusCls = "warning";
                                break;
                            case "Released":
                                statusCls = "success";
                                break;
                            case "Posted":
                                statusCls = "primary";
                                break;
                            case "Pending Approval":
                                statusCls = "success";
                                break;
                            case "Cancelled":
                                statusCls = "danger";
                                break;
                            case "Approved":
                                statusCls = "success";
                                break;
                        }
                        htmlStr += String.Format(@"
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>
                                <td>{4}</td>
                                <td>{5}</td>
                                <td>{6}</td>
                                <td>{7}</td>
                                <td><span class='label label-{9}'>{8}</span></td>
                                <td class='small'>
                                    <div class='options btn-group' >
					                    <a class='label label-success dropdown-toggle btn-success' data-toggle='dropdown' href='#' style='padding:4px;margin-top:3px'><i class='fa fa-gears'></i> Options</a>
					                    <ul class='dropdown-menu'>
                                            <li><a href='LeaveListing.aspx?leaveNo={1}&status={8}'><i class='fa fa-trash text-danger'></i><span class='text-danger'>Cancel</span></a></li>
                                            <li><a href='ApprovalTracking.aspx?DocNum={1}'><i class='fa fa-plus-circle text-success'></i><span class='text-success'>Approval Tracking</span></a></li>
                                        </ul>	
                                    </div>
                                </td>
                            </tr>
                            "
                        ,
                          counter,
                          responseArr[0],
                          responseArr[1],
                          responseArr[2],
                          responseArr[3],
                          responseArr[4],
                          responseArr[5],
                          responseArr[6],
                          responseArr[7],

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