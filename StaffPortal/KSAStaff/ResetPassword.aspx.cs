using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KSAStaff
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["staffNo"] == null)
                {
                    Response.Redirect("Default.aspx");
                    return;
                }
            }
        }

        private string GetStaffEmail(string username)
        {
            string email = string.Empty;
            try
            {
                string response = Components.ObjNav.GetStaffEmail(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] strLimiters = new string[] { "::" };
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    if (responseArr[0] == null) email = responseArr[1];
                    else email = responseArr[0];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return email;
        }

        private bool ValidPassword(string password)
        {
            bool valid = false;
            try
            {
                string pattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$";
                if (Regex.IsMatch(password, pattern))
                {
                    valid = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return valid;
        }

        private void Message(string message)
        {
            string strScript = "<script>alert('" + message + "');</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        private void SuccessMessage(string message)
        {
            string page = "Default.aspx";
            string strScript = "<script>alert('" + message + "');window.location='" + page + "';</script>";
            ClientScript.RegisterStartupScript(GetType(), "Client Script", strScript.ToString());
        }

        protected void lbtnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Request.QueryString["staffNo"].ToString();
                string newPassword = txtNewPass.Text;
                string confirmPassword = txtConfirmpassword.Text;
                string email = GetStaffEmail(username);

                if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    Message("Passwords cannot be empty");
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    Message("Passwords do not match");
                    return;
                }

                if (!ValidPassword(newPassword))
                {
                    Message("Password must be at least 6 characters, no more than 20 characters, and must include at least one upper case letter, one lower case letter, one numeric digit and a special character.");
                    return;
                }

                string response = Components.ObjNav.UpdateStaffPassword(username, newPassword);
                if (!string.IsNullOrEmpty(response))
                {
                    if (response == "SUCCESS")
                    {
                        string subject = "Kenya Scouts Association Portal Password";
                        string body = $"Your portals password has been reset successfully. Use below password to login." +
                            $"<br/><br/>" +
                            $"Portal password: <b>{newPassword}</b>";
                        Components.SentEmailAlerts(email, subject, body);
                        SuccessMessage($"Password has been reset successfully. A copy of the password has been sent to your email address {email.ToUpper()}");
                    }
                    else
                    {
                        Message("An error occured while updating password. Please try again later!");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}