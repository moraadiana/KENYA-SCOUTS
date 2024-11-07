using KSAStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KSAStaff.pages
{
    public partial class Dashboard : System.Web.UI.Page
    {
        Staffportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                LoadEmployeeDetails();
                LoadEmployeeProfile();
            }
        }

        private void LoadEmployeeProfile()
        {
            try
            {
                string username = Session["username"].ToString();
                string gender = webportals.GetStaffGender(username);
                string imgPath = string.Empty;
                if (gender == "Male") imgPath = "profile_m";
                if (gender == "Female") imgPath = "profile_f";
                ImgProfilePic.ImageUrl = $"~/images/{imgPath}.png";
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }

        private void LoadEmployeeDetails()
        {
            try
            {
                string username = Session["username"].ToString();
                string response = webportals.GetStaffDetails(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string returnMsg = responseArr[0];
                    if (returnMsg == "SUCCESS")
                    {
                        string staffNo = responseArr[1];
                        string gender = responseArr[2];
                        string idNumber = responseArr[3];
                        string email = responseArr[4];
                        string companyEmail = responseArr[5];
                        string phoneNumber = responseArr[6];
                        string citizenship = responseArr[7];
                        string postalCode = responseArr[8];
                        string postalAddress = responseArr[9];
                        string jobTitle = responseArr[10];
                        string staffName = Session["staffName"].ToString();

                        lblEmployeeNo.Text = staffNo;
                        lblGender.Text = gender;
                        lblIDNo.Text = idNumber;
                        lblEmail.Text = email;
                        lblEmailCompany.Text = companyEmail;
                        lblPhoneNo.Text = phoneNumber;
                        lblCitizenship.Text = citizenship;
                        lblPostalCode.Text = postalCode;
                        lblPostalAddress.Text = postalAddress;
                        lblDesignation.Text = jobTitle;
                        lblStaffName.Text = staffName;
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