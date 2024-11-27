using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Messaging;
using KSAStaff.NAVWS;
using System.Globalization;
using System.Drawing;

namespace KSAStaff.pages
{
    public partial class Pnine : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;
        SqlDataAdapter adapter;
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
                LoadYears();
                LoadP9();
            }
        }
        protected void LoadYears()
        {
            string username = Session["username"].ToString();
            //string nameu = "'"+username+"'";
            try
            {
                connection = Components.getconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetP9Years",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.Company_Name);
                command.Parameters.AddWithValue("@username", "'" + username + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    ddlYear.DataSource = reader;
                    ddlYear.DataTextField = "Period Year";
                    ddlYear.DataValueField = "Period Year";
                    ddlYear.DataBind();
                }
               
            }
            catch (Exception ex)
            {

                ex.Data.Clear();
            }
        }
        protected void LoadP9()
        {
            try
            {
                var filename = Session["username"].ToString().Replace(@"/", @"");
                var employee = Session["username"].ToString();
                // var myDate = "01" + "/01/" + ddlYear.SelectedValue;
                var period = Convert.ToInt32(ddlYear.SelectedValue);

                var filePath = Server.MapPath("~/Downloads/") + String.Format("P9Form-{0}.pdf", filename);
                if (!Directory.Exists(Server.MapPath("~/Downloads/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Downloads/"));
                }
                webportals.Generatep9Report(employee, period, String.Format(@"P9Form-{0}.pdf", filename));
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("P9 generated successfully.");
                    myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("P9Form-{0}.pdf", filename)));
                }
                else
                {
                    throw new FileNotFoundException("P9 PDF was not found after generation.");
                }
               // myPDF.Attributes.Add("src", ResolveUrl("~/Downloads/" + String.Format("P9Form-{0}.pdf", filename)));
                //try
                //{
                //    string returnstring = "";
                //   Components.ObjNav.Generatep9Report( employee, period, String.Format("p9Form{0}.pdf", filename));
                //    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("p9Form{0}.pdf", filename)));
                //    //WSConfig.ObjNavWS.FnFosaStatement(accno, ref returnstring, filter);
                //    byte[] bytes = Convert.FromBase64String(returnstring);
                //    string path = HostingEnvironment.MapPath("~/Download/" + $"p9Form{filename}.pdf");
                //    if (System.IO.File.Exists(path))
                //    {
                //        System.IO.File.Delete(path);
                //    }
                //    FileStream stream = new FileStream(path, FileMode.CreateNew);
                //    BinaryWriter writer = new BinaryWriter(stream);
                //    writer.Write(bytes, 0, bytes.Length);
                //    writer.Close();
                //    myPDF.Attributes.Add("src", ResolveUrl("~/Download/" + String.Format("p9Form{0}.pdf", filename)));
                //}
                //catch (Exception exception)
                //{
                //    exception.Data.Clear();
                //    //     HttpContext.Current.Response.Write(exception);
                //}
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
                //HttpContext.Current.Response.Write(ex);
            }
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadP9();
        }
    }
}