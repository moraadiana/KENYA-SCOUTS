using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KSAStaff.pages
{
    public partial class LeaveStatement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                GeneratetLeaveStatement();
            }
        }

        private void GeneratetLeaveStatement()
        {
            try
            {
                string username = Session["username"].ToString();
                string fileName = username.Replace("/", "");
                Components.ObjNav.GenerateStaffLeaveStatement(username, String.Format(@"Leave-Statement-{0}.pdf", fileName));
                myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format(@"Leave-Statement-{0}.pdf", fileName)));
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}