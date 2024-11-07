using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KIPREStaff.Layout
{
    public partial class Main : System.Web.UI.MasterPage
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
                lblUser.Text = Session["staffName"].ToString();
            }
        }

        protected void lbtnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                Session.RemoveAll();
                Session.Abandon();
                Session.Clear();
                Response.Redirect("~/Default.aspx");
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
        }
    }
}