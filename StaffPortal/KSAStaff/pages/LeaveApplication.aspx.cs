﻿using KSAStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KSAStaff.pages
{
    public partial class LeaveApplication : System.Web.UI.Page
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
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
                LoadLeaveTypes();
                LoadReliever();
                LoadResponsibilityCenter();
                LoadDays();
                //LoadLeaveBalance();
                LoadStaffDepartmentDetails();
            }
        }

        private void LoadStaffDepartmentDetails()
        {
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.GetStaffDepartmentDetails(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        lblDirectorate.Text = responseArr[1];
                        lblDepartment.Text = responseArr[2];
                    }
                    else
                    {
                        Message("An error occured while loading details. Please try again later.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadResponsibilityCenter()
        {           

                try
                {

                string grouping = "LEAVE";
                    ddlResponsibilityCenter.Items.Clear();
                    connection = Components.getconnToNAV();
                    command = new SqlCommand()
                    {
                        CommandText = "spLoadResponsibilityCentre",
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection
                    };
                    command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                    command.Parameters.AddWithValue("@grouping", "'"+grouping+"'");
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                                ListItem li = new ListItem(reader["Code"].ToString().ToUpper(), reader["Code"].ToString());
                                ddlResponsibilityCenter.Items.Add(li);
                           
                        }
                    }

                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
            }

        private void LoadReliever()
        {
            try
            {
                ddlReliver.Items.Clear();
                string username = Session["username"].ToString();
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetRelievers",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["No_"].ToString() == username) continue;
                        ListItem li = new ListItem(reader["Name"].ToString().ToUpper(), reader["No_"].ToString());
                        ddlReliver.Items.Add(li);
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadLeaveTypes()
        {
            try
            {
                string gender = Components.EmployeeGender;
                ddlLeaveType.Items.Clear();
                connection = Components.getconnToNAV();
               

                command = new SqlCommand()
                {
                    CommandText = "spGetLeaveTypes",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@gender", "'" + gender + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["Description"].ToString().ToUpper(), reader["Code"].ToString());
                        ddlLeaveType.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadLeaveBalance()
        {
            try
            {
                string leaveType = ddlLeaveType.SelectedValue;
                string employeeNo = Session["username"].ToString();
                string availableDays = webportals.AvailableLeaveDays(employeeNo, leaveType);
                if (!string.IsNullOrEmpty(availableDays))
                {
                    double leaveBalance = Convert.ToDouble(availableDays);
                    if (leaveBalance > 0)
                    {
                        lblBalance.Text = availableDays;
                        lbtnSubmit.Visible = true;
                    }
                    else
                    {
                        lblBalance.Text = "Not Available";
                        lbtnSubmit.Visible = false;
                    }
                }
                else
                {
                    lblBalance.Text = "Not Available";
                    lbtnSubmit.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void DefaultDays()
        {
            try
            {
                string leaveType = ddlLeaveType.SelectedValue.ToString();
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetDefaultDays",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@LeaveType", "'" + leaveType + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string days = Convert.ToDouble(Convert.ToDecimal(reader["Days"])).ToString();
                    lblBalance.Text = days;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private bool HasPendingApplication()
        {
            bool pendingApplication = false;
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.HasPendingLeaveApplication(username);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "Yes") pendingApplication = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return pendingApplication;
        }

        private void LoadDays()
        {
            try
            {
                string leaveType = ddlLeaveType.SelectedValue;
                string isLeaveAnnual = webportals.IsLeaveAnnual(leaveType);
                if (isLeaveAnnual == "Yes")
                {
                    LoadLeaveBalance();
                }
                else
                {
                    DefaultDays();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDays();
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string startDate = txtStartDate.Text;
                string appliedDays = txtAppliedDays.Text;
                string leaveType = ddlLeaveType.SelectedValue.ToString();

                if (string.IsNullOrEmpty(appliedDays))
                {
                    Message("Applied days cannot be empty");
                    txtAppliedDays.Focus();
                    return;
                }

                webportals.ValidateStartDate(Convert.ToDateTime(startDate));

                DateTime startingDate = Convert.ToDateTime(startDate);
                var endDate = webportals.CalcEndDate(startingDate, Convert.ToInt32(appliedDays), leaveType).ToString("d");
                var returnDate = webportals.CalcReturnDate(Convert.ToDateTime(endDate), leaveType).ToString("d");
                lblEndDate.Text = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");
                lblReturnDate.Text = Convert.ToDateTime(returnDate).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                txtStartDate.Text = string.Empty;
                lblEndDate.Text = string.Empty;
                lblReturnDate.Text = string.Empty;
                ex.Data.Clear();
            }
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"].ToString();
                string leaveType = ddlLeaveType.SelectedValue;
                string reliever = ddlReliver.SelectedValue;
                string resCenter = ddlResponsibilityCenter.SelectedValue;
                string appliedDays = txtAppliedDays.Text;
                string startDate = txtStartDate.Text;
                string directorate = lblDirectorate.Text;
                string department = lblDepartment.Text;
                string availableDays = lblBalance.Text;
                string purpose = txtPurpose.Text;

                // Validations
                if (HasPendingApplication())
                {
                    SuccessMessage("You have a Pending Leave Application. Please Cancel/Wait for it to be Approved and Try Again.");
                    return;
                }

                if (string.IsNullOrEmpty(leaveType))
                {
                    Message("Leave type cannot be null");
                    return;
                }

                if (string.IsNullOrEmpty(reliever))
                {
                    Message("Reliever cannot be null");
                    return;
                }

                if (string.IsNullOrEmpty(resCenter))
                {
                    Message("Responsibility center cannot be null");
                    return;
                }

                if (string.IsNullOrEmpty(appliedDays))
                {
                    Message("Applied days cannot be null");
                    txtAppliedDays.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(startDate))
                {
                    Message("Start date cannot be null");
                    return;
                }
                if (string.IsNullOrEmpty(directorate))
                {
                    Message("KSA cannot be null");
                    return;
                }
                if (string.IsNullOrEmpty(department))
                {
                    Message("Department cannot be null");
                    return;
                }
                if (string.IsNullOrEmpty(purpose))
                {
                    Message("Purpose cannot cannot be null");
                    return;
                }

                if (purpose.Length > 200)
                {
                    Message("Purpose cannot be more than 200 characters");
                    return;
                }

                if (Convert.ToInt32(availableDays) < Convert.ToInt32(appliedDays))
                {
                    Message("Applied days cannot be more than available days!");
                    return;
                }
                else if (Convert.ToInt32(availableDays) <= 0)
                {
                    Message("You have exhausted your leave days. Please visit the HR to update your leave days");
                    return;
                }
                else if (availableDays == "Not Available")
                {
                    Message("You have no leave days assigned to the leave type. Please select another leave type!");
                    return;
                };

                DateTime endDate = Convert.ToDateTime(lblEndDate.Text);
                DateTime returnDate = Convert.ToDateTime(lblReturnDate.Text);

                // Applications
                string response = webportals.HRMLeaveApplication(username, reliever, leaveType, Convert.ToDecimal(appliedDays), Convert.ToDateTime(startDate), endDate, returnDate, purpose, resCenter,directorate,department);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string leaveNo = responseArr[1];
                        string approval = webportals.OnsendLeaveRequisitionForApproval(leaveNo);
                        if (approval == "SUCCESS")
                        {
                            // TODO: Sent email to reliever
                            SuccessMessage("Leave application has been sent for approval successfully.");
                            return;
                        }
                        else
                        {
                            Message(approval);
                            return;
                        }
                    }
                    else
                    {
                        Message("An error occured while applying for the leave. Please try again later!");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void SuccessMessage(string message)
        {
            string page = "LeaveListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }
    }
}