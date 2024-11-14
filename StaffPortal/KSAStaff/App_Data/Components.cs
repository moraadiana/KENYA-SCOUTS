using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using KSAStaff.NAVWS;

namespace KSAStaff
{
    public class Components
    {
        public static SqlConnection connection;
        public static string Company_Name = "KSA";
        //public static string ServiceRoot = "http://108.181.152.166:9048/KSCOUTS/ODataV4/Company('KSA')/";

        //public static void Context_BuildingRequest(object sender, Microsoft.OData.Client.BuildingRequestEventArgs e)
        //{
        //    e.Headers.Add("Authorization", "Basic d2VicG9ydGFsczpXZWJwb3J0YWxzQDIwMjQ=");
        //}
        public static Staffportal ObjNav
        {
            get
            {
                var ws = new Staffportal();
                try
                {
                    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["W_USER"], ConfigurationManager.AppSettings["W_PWD"]);
                    ws.Credentials = credentials;
                    ws.PreAuthenticate = true;
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }
                return ws;
            }
        }

        public static void SentEmailAlerts(string address, string subject, string message)
        {
            try
            {
                //string email = "dynamicsselfservice@gmail.com";
                //string password = "";

                //var loginInfo = new NetworkCredential(email, password);
                //var msg = new MailMessage();
                //var smtpClient = new SmtpClient("smtp.gmail.com", 25);

                string smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                string smtpUser = ConfigurationManager.AppSettings["SMTPUser"];
                string smtpPass = ConfigurationManager.AppSettings["SMTPPassword"];
                string fromAddress = ConfigurationManager.AppSettings["FromAddress"];

                var loginInfo = new NetworkCredential(smtpUser, smtpPass);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient(smtpServer, smtpPort);
                msg.From = new MailAddress(fromAddress);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception Ex)
            {
                Ex.Data.Clear();
            }
        }

        public static SqlConnection getconnToNAV()
        {
            try
            {
                if (connection == null || connection.State == ConnectionState.Closed)
                {
                    var sqlConnectionString = ConfigurationManager.AppSettings["SqlConnection"];

                    connection = new SqlConnection(sqlConnectionString);

                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return connection;
        }

        /* public static string EmployeeGender
         {
             get
             {
                 string s = "";

                 try
                 {
                     string strSQL = String.Format("SELECT [Gender] FROM [" + Components.Company_Name + "$HRM-Employee C$b81538f8-8ab8-4c54-b747-1d1b3033fd21] WHERE No_ = '" + HttpContext.Current.Session["username"].ToString() + "'");
                     SqlCommand command = new SqlCommand(strSQL, getconnToNAV());
                     using (SqlDataReader dr = command.ExecuteReader())
                     {
                         if (dr.HasRows)
                         {
                             dr.Read();
                             s = (Convert.ToInt32(dr["Gender"])).ToString();
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     ex.Data.Clear();
                 }
                 return s;
             }
         }*/
        public static string EmployeeGender
        {
            get
            {
                string genderValue = "";

                try
                {
                    // Retrieve username from session
                    var username = HttpContext.Current.Session["username"];
                    if (username == null || string.IsNullOrEmpty(username.ToString()))
                    {
                        throw new Exception("Session variable 'username' is not set or is empty.");
                    }

                    // Initialize the web service client for GetStaffGender
                    var client = new Staffportal(); // Replace with actual client name
                    client.Credentials = new NetworkCredential("webportals", "Webportals@2024");
                    genderValue = client.GetStaffGender(username.ToString()); // Call the AL procedure via web service

                    if (string.IsNullOrEmpty(genderValue))
                    {
                        Console.WriteLine("Gender not returned for user: " + username.ToString());
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception to see what went wrong
                    Console.WriteLine("Error retrieving EmployeeGender: " + ex.Message);
                }

                return genderValue;
            }
        }

        public static bool IsNumeric(string no)
        {
            double result;
            if (double.TryParse(no, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}