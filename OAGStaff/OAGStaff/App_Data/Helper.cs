using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff
{
    public class Helper
    {
        private static Staffportal webportal = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };
        public static List<App> GetAllEmployees()
        {
            var list = new List<App>();
            try
            {
                string employees = webportal.GetEmployees();
                if (!string.IsNullOrEmpty(employees))
                {
                    string[] employeesArr = employees.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string employee in employeesArr)
                    {
                        string[] responseArr = employee.Split(strLimiters, StringSplitOptions.None);
                        string username = HttpContext.Current.Session["username"].ToString();
                        if (responseArr[0] == username) continue;
                        App emp = new App()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                        };
                        list.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<App> GetResponsibilityCenters()
        {
            var list = new List<App>();
            try
            {
                string resCenters = webportal.GetResponsibilityCenters();
                if (!String.IsNullOrEmpty(resCenters))
                {
                    string[] resCentersArr = resCenters.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string resCenter in resCentersArr)
                    {
                        string[] responseArr = resCenter.Split(strLimiters, StringSplitOptions.None);
                        App app = new App()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                        };
                        list.Add(app);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }

            return list;
        }

        public static App GetEmployeeDepartmentDetails(string username)
        {
            App app = new App();
            try
            {
                string response = webportal.GetStaffDepartmentDetails(username);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    app.Department = responseArr[0];
                    app.Directorate = responseArr[1];
                    app.SubDirectorate = responseArr[2];
                    app.UnitCode = responseArr[3];
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return app;
        }

        public static string VerifyCurrentPassword(string username, string password)
        {
            string verify = string.Empty;
            try
            {
                bool response = webportal.ValidateOldPassword(username, password);
                verify = response.ToString();
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return verify;
        }
    }
}