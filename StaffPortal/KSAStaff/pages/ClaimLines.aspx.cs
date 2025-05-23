﻿using KSAStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KSAStaff.pages
{
    public partial class ClaimLines : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter adapter;
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                LoadStaffDetails();
                LoadResponsibilityCenter();

                string query = Request.QueryString["query"];
                string approvalStatus = Request.QueryString["status"].Replace("%", " ");
                if (query == "new")
                {
                    MultiView1.SetActiveView(vwHeader);
                }
                else if (query == "old")
                {
                    string claimNo = Request.QueryString["ClaimNo"];
                    MultiView1.SetActiveView(vwLines);
                    lblLNo.Text = claimNo;
                    lblClaimNo.Text = claimNo;
                    LoadAdvanceTypes();
                    BindGridViewData(claimNo);
                    BindAttachedDocuments(claimNo);
                }

                if (approvalStatus == "Open" || approvalStatus == "Pending")
                {
                    btnApproval.Visible = true;
                    btnCancellApproval.Visible = false;
                    attachments.Visible = true;
                }
                else if (approvalStatus == "Pending Approval")
                {
                    btnApproval.Visible = false;
                    btnCancellApproval.Visible = true;
                    lbtnAddLine.Visible = false;
                    lbtnClose.Visible = false;
                    attachments.Visible = false;
                }
                else
                {
                    btnApproval.Visible = false;
                    btnCancellApproval.Visible = false;
                    lbtnAddLine.Visible = false;
                    lbtnClose.Visible = false;
                    attachments.Visible = false;
                }
            }
        }

        private void LoadStaffDetails()
        {
            try
            {
                string staffNo = Session["username"].ToString();
                string staffName = Session["StaffName"].ToString();
                string response = webportals.GetStaffDepartmentDetails(staffNo);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                       // lblDirectorate.Text = responseArr[2];
                        lblDepartment.Text = responseArr[1];
                    }
                }

                lblStaffNo.Text = staffNo;
                lblPayee.Text = staffName;
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
                string grouping = "CLAIM";
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
        private void LoadAdvanceTypes()
        {
            try
            {
                ddlAdvancType.Items.Clear();
                string advanceTypes = webportals.GetAdvancetype(4);
                if (!string.IsNullOrEmpty(advanceTypes))
                {
                    string[] advanceTypeArr = advanceTypes.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string advanceType in advanceTypeArr)
                    {
                        string[] responseArr = advanceType.Split(strLimiters, StringSplitOptions.None);
                        ListItem li = new ListItem(responseArr[0]);
                        ddlAdvancType.Items.Add(li);
                    }
                }

            }


            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }



        private void LoadAdvanceTypes1()
        {
            try
            {
                ddlAdvancType.Items.Clear();
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spLoadClaimAdvancedTypes",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem li = new ListItem(reader["Description"].ToString().ToUpper(), reader["Code"].ToString());
                        ddlAdvancType.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Session["username"].ToString();
                string department = lblDepartment.Text;
               // string directorate = lblDirectorate.Text;
                string responsibilityCenter = lblResCenter.Text;
                string purpose = txtPurpose.Text;

                if (string.IsNullOrEmpty(department))
                {
                    Message("Department cannot be null!");
                    return;
                }
                //if (string.IsNullOrEmpty(directorate))
                //{
                //    Message("Division cannot be null!");
                //    return;
                //}
                if (string.IsNullOrEmpty(responsibilityCenter))
                {
                    Message("Responsibility center cannot be null!");
                    return;
                }
                if (purpose == "")
                {
                    Message("Purpose cannot be null!");
                    txtPurpose.Focus();
                    return;
                }
                if (purpose.Length > 200)
                {
                    Message("Purpose should be 200 characters and below!");
                    return;
                }
                
                string response = webportals.CreateClaimRequisitionHeader(username, department, responsibilityCenter, purpose);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string claimNo = responseArr[1];
                        Message($"Claim number {claimNo} has been created successfully!");
                        Session["ClaimNo"] = claimNo;
                        BindGridViewData(claimNo);
                        NewView();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void NewView()
        {
            try
            {
                MultiView1.SetActiveView(vwLines);
                string claimNo = Session["ClaimNo"].ToString();
                newLines.Visible = true;
                lbtnAddLine.Visible = false;
                lblLNo.Text = claimNo;
                lblClaimNo.Text = claimNo;
                LoadAdvanceTypes();
                BindGridViewData(claimNo);
                BindAttachedDocuments(claimNo);

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void BindAttachedDocuments(string claimNo)
        {
            try
            {
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spDocumentLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@ReqNo", "'" + claimNo + "'");
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvAttachments.DataSource = dt;
                gvAttachments.DataBind();
                connection.Close();

                foreach (GridViewRow row in gvAttachments.Rows)
                {
                    row.Cells[3].Text = row.Cells[3].Text.Split(' ')[0];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void BindGridViewData(string claimNo)
        {
            try
            {
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spClaimLines",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@ClmNo", "'" + claimNo + "'");
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvLines.DataSource = dt;
                gvLines.DataBind();
                connection.Close();

                foreach (GridViewRow row in gvLines.Rows)
                {
                    row.Cells[6].Text = Convert.ToDateTime(row.Cells[6].Text).ToShortDateString();

                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "')</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void SuccessMessage(string message)
        {
            string page = "ClaimListing.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "'</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        protected void btnLine_Click(object sender, EventArgs e)
        {
            try
            {
                string claimNo = lblClaimNo.Text;
                string employeeNo = Session["username"].ToString();
                string advanceType = ddlAdvancType.SelectedValue;
                string amount = txtAmnt.Text;
                if (advanceType == "0")
                {
                    Message("Advance type cannot be null!");
                    return;
                }
                if (amount == "")
                {
                    Message("Amount cannot be empty!");
                    txtAmnt.Focus();
                    return;
                }

                string claimLine = webportals.InsertClaimRequisitionLines(claimNo, advanceType, Convert.ToDecimal(amount));
                if (!string.IsNullOrEmpty(claimLine))
                {
                    string[] strLimiters = new string[] { "::" };
                    string[] claimLinesArr = claimLine.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = claimLinesArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        Message("Line added successfully!");
                        txtAmnt.Text = string.Empty;
                        BindGridViewData(claimNo);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnAddLine_Click(object sender, EventArgs e)
        {
            string claimNo = string.Empty;
            if (Request.QueryString["ClaimNo"] == null)
            {
                claimNo = Session["ClaimNo"].ToString();
            }
            else
            {
                claimNo = Request.QueryString["ClaimNo"].ToString();
            }
            lblLNo.Text = claimNo;
            LoadAdvanceTypes();
            newLines.Visible = true;
            lbtnAddLine.Visible = false;
        }

        protected void lbtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string status = Request.QueryString["status"].ToString().Replace("%", " ");
                if (status == "Open" || status == "Pending")
                {
                    string[] args = new string[2];
                    string claimNo = lblClaimNo.Text;
                    args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                    string lineNo = args[0];
                    string response = webportals.RemoveClaimRequisitionLines(Convert.ToInt32(lineNo));
                    if (!string.IsNullOrEmpty(response))
                    {
                        if (response == "SUCCESS")
                        {
                            Message("Line deleted successfully!");
                            BindGridViewData(claimNo);
                        }
                        else
                        {
                            Message("An error occured while removing line. Please try again later");
                            return;
                        }
                    }
                }
                else
                {
                    Message("You can only edit an open document!");
                    return;
                }

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void btnApproval_Click(object sender, EventArgs e)
        {
            try
            {
                string claimNo = lblClaimNo.Text;
                if (gvLines.Rows.Count < 1)
                {
                    Message("Please add lines before sending for approval!");
                    return;
                }
                if (gvAttachments.Rows.Count < 1)
                {
                    Message("Please attach documents before sending for approval!");
                    return;
                }
                string msg = webportals.OnSendClaimRequisitionForApproval(claimNo);
                if (msg == "SUCCESS")
                {
                    SuccessMessage($"Claim number {claimNo} has been sent for approval successfuly!");
                }
                else
                {
                    Message("ERROR: " + msg);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void btnCancellApproval_Click(object sender, EventArgs e)
        {
            try
            {
                string claimNo = lblClaimNo.Text;
                webportals.OnCancelClaimRequisition(claimNo);
                SuccessMessage($"Claim number {claimNo} has been cancelled successfuly!");
            }
            catch (Exception ex)
            {
                Message("ERROR: " + ex.Message);
                ex.Data.Clear();
            }
        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            newLines.Visible = false;
            lbtnAddLine.Visible = true;
        }

        protected void lbtnAttach_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuClaimDocs.PostedFile != null)
                {
                    string DocumentNo = lblLNo.Text;
                    string username = Session["username"].ToString();
                    string filePath = fuClaimDocs.PostedFile.FileName.Replace(" ", "-");
                    string fileName = fuClaimDocs.FileName.Replace(" ", "-");
                    string fileExtension = Path.GetExtension(fileName).Split('.')[1].ToLower();
                    if (fileExtension == "pdf" || fileExtension == "jpg" || fileExtension == "png" || fileExtension == "jpeg")
                    {
                        string strPath = Server.MapPath("~/Uploads");
                        if (!Directory.Exists(strPath))
                        {
                            Directory.CreateDirectory(strPath);
                        }

                        string pathToUpload = Path.Combine(strPath, DocumentNo + fileName.ToUpper());
                        if (File.Exists(pathToUpload))
                        {
                            File.Delete(pathToUpload);
                        }
                        fuClaimDocs.SaveAs(pathToUpload);
                        webportals.SaveMemoAttchmnts(DocumentNo, pathToUpload, fileName.ToUpper(), username);
                        Stream fs = fuClaimDocs.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        webportals.RegFileUploadAtt(DocumentNo, fileName.ToUpper(), base64String, 52178720, "Claim Requisition");
                        BindAttachedDocuments(DocumentNo);
                        Message("Document uploaded successfully!");
                    }
                    else
                    {
                        Message("Please upload files with .pdf, .png, .jpg and .jpeg extensions only!");
                        return;
                    }
                }
                else
                {
                    Message("Please upload a file!");
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnRemoveAttach_Click(object sender, EventArgs e)
        {
            string status = Request.QueryString["status"].ToString().Replace("%", " ");
            if (status == "Open" || status == "Pending")
            {
                string[] args = new string[2];
                args = (sender as LinkButton).CommandArgument.ToString().Split(';');
                string systemId = args[0];
                string documentNo = lblLNo.Text;
                string fileName = string.Empty;
                string documentDetails = webportals.GetAttachmentDetails(systemId);
                if (documentDetails != null)
                {
                    string[] documentsDetailsArr = documentDetails.Split(strLimiters, StringSplitOptions.None);
                    fileName = documentsDetailsArr[1].Split('.')[0];
                }

                string response = webportals.DeleteDocumentAttachment(systemId, fileName, documentNo);
                if (response != null)
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg1 = responseArr[0];
                    string returnMsg2 = responseArr[1];
                    if (returnMsg1 == "SUCCESS" && returnMsg2 == "SUCCESS")
                    {
                        Message("Document deleted successfully.");
                        BindAttachedDocuments(documentNo);
                    }
                    else
                    {
                        Message("An error has occured. Please try again later.");
                        return;
                    }
                }
            }
            else
            {
                Message("You can only edit an open document!");
                return;
            }
        }
    }
}