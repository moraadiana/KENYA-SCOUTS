using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace OAGStaff
{
    public class Components
    {
        public static SqlConnection connection;
        public static string Company_Name = "OAG TEST";
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

        public static void SentEmailAlerts(string address, string subject, string message)
        {
            try
            {
                string email = "dynamicsselfservice@gmail.com";
                string password = "ydujienvejtdojgv";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 25);

                msg.From = new MailAddress(email);
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

        public static string GenerateRandomId()
        {
            string id = string.Empty;
            try
            {
                string num = "1234567890abcdefghijklmnopqrstuvwxyz";
                int len = num.Length;
                int otpdigit = 6;
                string finaldigit;

                int getindex;

                for (int i = 0; i <= otpdigit; i++)
                {
                    do
                    {
                        getindex = new Random().Next(0, len);
                        finaldigit = num.ToCharArray()[getindex].ToString();
                    }
                    while (id.IndexOf(finaldigit) != -1);
                    id += finaldigit;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return id.ToUpper();
        }

        public static bool ValidPassword(string password)
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

        public static string StatusClass(string status)
        {
            string statusCls = "default";
            switch (status)
            {
                case "Open":
                    statusCls = "warning";
                    break;
                case "Pending Approval":
                    statusCls = "primary";
                    break;
                case "Approved":
                    statusCls = "success";
                    break;
                case "Canceled":
                    statusCls = "danger";
                    break;
                case "Posted":
                    statusCls = "success";
                    break;
                default:
                    statusCls = "info";
                    break;
            }
            return statusCls;
        }
    }
}