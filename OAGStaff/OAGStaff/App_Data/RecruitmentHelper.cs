using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAGStaff.Models;
using OAGStaff.NAVWS;

namespace OAGStaff
{
    public class RecruitmentHelper
    {
        private static Staffportal webportals = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };

        public static List<Recruitment> GetRequisitions()
        {
            var list = new List<Recruitment>();
            try
            {
                string requisitions = webportals.GetEmployeeRequisitions();
                if(!string.IsNullOrEmpty(requisitions) )
                {
                    string[] requisitionsArr = requisitions.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach( string requisition in requisitionsArr)
                    {
                        string[] responseArr = requisition.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Recruitment()
                        {
                            Code = responseArr[0],
                            Description = $"{responseArr[0]} => {responseArr[1]} => {responseArr[2]}"
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Recruitment> GetApplicants(string requisitionNo)
        {
            var list = new List<Recruitment>();
            try
            {
                string requisitions = webportals.GetApplicants(requisitionNo);
                if (!string.IsNullOrEmpty(requisitions))
                {
                    string[] requisitionsArr = requisitions.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requisition in requisitionsArr)
                    {
                        string[] responseArr = requisition.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Recruitment()
                        {
                            ApplicantNo = responseArr[0],
                            ApplicantName = responseArr[1]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }
    }
}