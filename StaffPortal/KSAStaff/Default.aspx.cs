using KSAStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KSAStaff
{
    public partial class _Default : Page
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        protected void Page_Load(object sender, EventArgs e)
        {
            txtusername.Focus();
        }

        private bool ValidStaffNo(string username)
        {
            bool valid = false;
            try
            {
                string response = webportals.CheckValidStaffNo(username);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "Valid")
                    {
                        valid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return valid;
        }

        private bool ChangedPassword(string username)
        {
            bool changed = false;
            try
            {
                string response = webportals.CheckStaffPasswordChanged(username);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "Yes")
                    {
                        changed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return changed;
        }

        private void LoginForChangedPassword(string username, string password)
        {
            try
            {
                string response = webportals.StaffLogin(username, password);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string staffNo = responseArr[1];
                        string staffName = responseArr[2];
                        Session["username"] = staffNo;
                        Session["staffName"] = staffName;
                        Response.Redirect("pages/Dashboard.aspx");
                    }
                    else
                    {
                        lblError.Text = returnMsg;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoginForUnchangedPassword(string username)
        {
            try
            {
                string response = webportals.LoginForUnchnagedPassword(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string staffNo = responseArr[1];
                        string staffEmail = responseArr[2];
                        Response.Redirect($"ResetPassword.aspx?staffNo={staffNo}&email={staffEmail}");
                    }
                    else
                    {
                        lblError.Text = returnMsg;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private string GetStaffEmail(string username)
        {
            string staffEmail = string.Empty;
            try
            {
                string response = webportals.GetStaffEmail(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string email = string.Empty;
                    if (responseArr[0] != null) email = responseArr[1];
                    else email = responseArr[0];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return staffEmail;
        }

        private string GetStaffPassword(string username)
        {
            string password = string.Empty;
            try
            {
                string response = webportals.GetStaffPassword(username);
                if (!string.IsNullOrEmpty(response))
                {
                    password = response;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return password;
        }

        protected void lbtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtusername.Value.Trim();
                string password = txtpassword.Value.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    lblError.Text = "Username and password cannot be empty";
                    return;
                }

                if (!ValidStaffNo(username))
                {
                    lblError.Text = "Invalid Staff No.";
                    return;
                }

                if (ChangedPassword(username))
                {
                    LoginForChangedPassword(username, password);
                }
                else
                {
                    LoginForUnchangedPassword(username);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        protected void lbtnForgot_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtusername.Value.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    lblError.Text = "Username cannot be null";
                    txtusername.Focus();
                    return;
                }

                if (!ValidStaffNo(username))
                {
                    lblError.Text = "Invalid Staff No";
                    return;
                }

                string email = GetStaffEmail(username);
                string staffPassword = GetStaffPassword(username);
                string subject = "KSA Portal Password Reset";
                string body = $"Use below password to log into your portal.<br/><br/>Portal password: {staffPassword}.<br/><br/>Do not reply to this email.";
                Components.SentEmailAlerts(email, subject, body);
                lblError.Text = $"Portal password has been sent to your email address {email.ToUpper()}";
                return;
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}
