using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KSAStaff.pages
{
    public partial class MemoReport : System.Web.UI.Page
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

                GenerateMemoReport();
            }
        }
        private void GenerateMemoReport()
        {
            try
            {
                string memoNo = Request.QueryString["memoNo"].ToString();
                string username = Session["username"].ToString();
                string filename = username.Replace("/", "");
                string pdfFileName = String.Format(@"MemoReport-{0}.pdf", filename);

                Components.ObjNav.GenerateMemoReport(memoNo, pdfFileName);

                string networkPath = @"\\10.107.8.40\Downloads\" + pdfFileName;


                string localFolderPath = Server.MapPath("~/Downloads/");
                string localFilePath = Path.Combine(localFolderPath, pdfFileName);


                if (!Directory.Exists(localFolderPath))
                {
                    Directory.CreateDirectory(localFolderPath);
                }


                System.IO.File.Copy(networkPath, localFilePath, true);


                string fileUrl = ResolveUrl("~/Downloads/" + pdfFileName);


                myPDF.Attributes.Add("src", fileUrl);
            }
            catch (Exception ex)
            {

                ex.Data.Clear();
            }
        }


    }
}
