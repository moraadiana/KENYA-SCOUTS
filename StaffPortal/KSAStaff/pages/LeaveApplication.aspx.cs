﻿using KSAStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Messaging;
using System.Web.Configuration;

namespace KSAStaff.pages
{
    public partial class LeaveApplication : System.Web.UI.Page
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters2 = new string[] { "[]" };
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
                string query = Request.QueryString["query"];
                string leaveNo = null;
                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                LoadLeaveTypes();
                LoadReliever();
                LoadResponsibilityCenter();
                //LoadDays();
                LoadLeaveBalance();
                LoadStaffDepartmentDetails();
                if (query == "new")
                {
                    leaveNo = null;
                    lbtnSubmit.Visible = true;
                }
                else if (query == "old")
                {
                     leaveNo = Request.QueryString["leaveNo"].ToString();
                    string response = webportals.GetLeaveDetails(leaveNo);
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] responseArr = response.Split(':');
                        string EmployeeNo = responseArr[0];
                        string EmployeeName = responseArr[1];
                        string Date = responseArr[2];
                        string AppliedDays = responseArr[3];
                        string StartingDate = responseArr[4];
                        string EndingDate = responseArr[5];
                        string Purpose = responseArr[6];
                        string LeaveType = responseArr[7];
                        string ReturnDate = responseArr[8];
                        string userId = responseArr[9];
                        string RelieverNo = responseArr[10];
                        string RelieverName = responseArr[11];
                        string department = responseArr[12];
                        ddlLeaveType.SelectedValue = LeaveType;
                        txtAppliedDays.Text = AppliedDays;

                        txtPurpose.Text = Purpose;
                        //lblEndDate.Text = EndingDate;


                        if (!ddlReliver.Items.Contains(new ListItem(RelieverNo + " => " + RelieverName, RelieverNo)))
                        {
                            ListItem li = new ListItem(RelieverNo + " => " + RelieverName, RelieverNo);
                            ddlReliver.Items.Add(li);
                        }



                    }

                    if (approvalStatus == "Open" || approvalStatus == "Pending")
                    {
                       
                        lbtnSubmit.Visible = true;
                    }
                    
                    else
                    {
                        lbtnSubmit.Visible = false;
                    }
                 }
               
               

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
                        //lblDirectorate.Text = responseArr[1];
                        lblDepartment.Text = responseArr[1];
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
                string responsibilityCenters = webportals.GetDocResponsibilityCentres(grouping);

                if (!string.IsNullOrEmpty(responsibilityCenters))
                {
                    string[] centers = responsibilityCenters.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);

                    if (centers.Length > 0)
                    {
                        lblResCenter.Text = centers[0];
                    }
                }
                else
                {
                    lblResCenter.Text = "No responsibility centers found.";
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                lblResCenter.Text = "Error loading responsibility centers.";
            }
        }

        private void LoadReliever()
        {
            try
            {
                ddlReliver.Items.Clear();
                string Relievers = webportals.GetRelievers();
                if (!string.IsNullOrEmpty(Relievers))
                {
                    string[] relieverArr = Relievers.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string reliever in relieverArr)
                    {
                        string[] responseArr = reliever.Split(strLimiters, StringSplitOptions.None);
                        ListItem li = new ListItem(responseArr[1], responseArr[0]);
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
                string LeaveTypeList = webportals.GetLeaveTypes(gender);

                if (!string.IsNullOrEmpty(LeaveTypeList))
                {

                    string[] LeaveTypeLines = LeaveTypeList.Split('|');


                    ddlLeaveType.Items.Clear();

                    foreach (string LeaveTypeLine in LeaveTypeLines)
                    {

                        string[] details = LeaveTypeLine.Split(new string[] { "::" }, StringSplitOptions.None);
                        if (details.Length == 2)
                        {
                            string Code = details[0];
                            string Description = details[1];

                            // Add new item to the dropdown list
                            System.Web.UI.WebControls.ListItem listItem = new System.Web.UI.WebControls.ListItem($"{Code} - {Description}", Code);
                            ddlLeaveType.Items.Add(listItem);
                        }
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
                LoadLeaveBalance();
                string leaveType = ddlLeaveType.SelectedValue;
                string defaultDays = webportals.GetDefaultDays(leaveType);
                if (!string.IsNullOrEmpty(defaultDays))
                {
                    lblBalance.Text = defaultDays;
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

       
        protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
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
                    //LoadLeaveBalance();
                }
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
                DateTime applicationDate = DateTime.Now;

                // Ensure applied days is not empty
                if (string.IsNullOrEmpty(appliedDays))
                {
                    Message("Applied days cannot be empty.");
                    txtAppliedDays.Focus();
                    return;
                }

                // Validate and parse start date
                if (!DateTime.TryParse(startDate, out DateTime startingDate))
                {
                    Message("Invalid date format. Please enter a valid start date.");
                    txtStartDate.Text = string.Empty;
                    return;
                }

                // Check if the start date is at least 3 days from the application date
                if ((startingDate - applicationDate).TotalDays < 3)
                {
                    Message("Start date must be at least 3 days from the application date.");
                    txtStartDate.Text = string.Empty;
                    return;
                }

                // Validate start date using webportals
                webportals.ValidateStartDate(startingDate);

                // Calculate end date and return date
                var endDate = webportals.CalcEndDate(startingDate, Convert.ToInt32(appliedDays), leaveType).ToString("yyyy-MM-dd");
                var returnDate = webportals.CalcReturnDate(Convert.ToDateTime(endDate), leaveType).ToString("yyyy-MM-dd");

                // Update labels
                lblEndDate.Text = endDate;
                lblReturnDate.Text = returnDate;
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                txtStartDate.Text = string.Empty;
                lblEndDate.Text = string.Empty;
                lblReturnDate.Text = string.Empty;
            }
        }

       

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"].ToString();
                string leaveType = ddlLeaveType.SelectedValue;
                string reliever = ddlReliver.SelectedValue;
                string resCenter = lblResCenter.Text;
                string appliedDays = txtAppliedDays.Text;
                string startDate = txtStartDate.Text;
               // string directorate = lblDirectorate.Text;
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
                //if (string.IsNullOrEmpty(directorate))
                //{
                //    Message("KSA cannot be null");
                //    return;
                //}
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
                string LeaveNo = null;
                string query = Request.QueryString["query"];
                if (query == "old")
                {
                    LeaveNo = Request.QueryString["leaveNo"]; // Get the leaveNo from the query string
                }

                // Applications
                string response = webportals.HRMLeaveApplication1(LeaveNo?? string.Empty, username, reliever, leaveType, Convert.ToDecimal(appliedDays), Convert.ToDateTime(startDate), endDate, returnDate, purpose, resCenter);
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
                            string relieverEmail = webportals.GetRelieverEmail(reliever);
                            string subject = "KSA Leave Relieval Request";
                            string body = $"Hello {ddlReliver.SelectedItem}" +
                                $"<br/><br/>" +
                                $"You have been requested by {Session["staffName"]} to be a reliever." +
                                $"<br/><br/>" +
                                $"Do not reply to this email.";
                            Components.SentEmailAlerts(relieverEmail, subject, body);
                            SuccessMessage("Leave application has been sent for approval successfully.");
                            return;
                        }
                        else
                        {
                            SuccessMessage(approval);
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