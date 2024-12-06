using OAGJobs.Models;
using OAGJobs.NAVWS;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace OAGJobs
{
    public class Services
    {
        private static Recruitment webportals = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };
        
        public static List<Applicant> GetApplicantAttachments(string applicationNo)
        {
            var qualificationTypes = new List<Applicant>();
            try
            {
                string qualifications = webportals.GetApplicantAttachments(applicationNo);
                if (!string.IsNullOrEmpty(qualifications))
                {
                    int counter = 0;
                    string[] qualificationsArr = qualifications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string qualification in qualificationsArr)
                    {
                        counter++;
                        string[] responseArr = qualification.Split(strLimiters, StringSplitOptions.None);
                        Applicant qualificationType = new Applicant()
                        {
                            DocumentNo = responseArr[0],
                            Description = responseArr[1],
                            CreatedAt = Convert.ToDateTime(responseArr[2]),
                            SystemId = responseArr[3],
                            Counter = counter
                        };
                        qualificationTypes.Add(qualificationType);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return qualificationTypes;
        }

        public static List<Registration> GetNationalities()
        {
            var list = new List<Registration>();
            try
            {
                string nationalities = webportals.GetNationalities();
                if(!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string nationality in nationalitiesArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Registration() 
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Applicant> GetQualificationTypes(string jobId)
        {
            var list = new List<Applicant>();
            try
            {
                string nationalities = webportals.GetJobQualificationType(jobId);
                if (!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    var distinctArr = nationalitiesArr.Distinct();
                    foreach (string nationality in distinctArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            QualificationType = responseArr[0],
                            JobId = responseArr[1]
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

        public static List<Applicant> GetQualificationCodes(string jobId, string qualificationType)
        {
            var list = new List<Applicant>();
            try
            {
                string nationalities = webportals.GetJobQualificationCodes(jobId, qualificationType);
                if (!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    var distinctArr = nationalitiesArr.Distinct();
                    foreach (string nationality in distinctArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            QualificationCode = responseArr[0],
                            QualificationDescription = responseArr[1]
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

        public static List<Applicant> GetRelationships()
        {
            var list = new List<Applicant>();
            try
            {
                string nationalities = webportals.GetRelatives();
                if (!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string nationality in nationalitiesArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Registration> GetTribes()
        {
            var list = new List<Registration>();
            try
            {
                string nationalities = webportals.GetTribes();
                if (!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string nationality in nationalitiesArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Registration()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Registration> GetCounties()
        {
            var list = new List<Registration>();
            try
            {
                string nationalities = webportals.GetCounties();
                if (!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string nationality in nationalitiesArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Registration()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Registration> GetSubCounties(string county)
        {
            var list = new List<Registration>();
            try
            {
                string nationalities = webportals.GetSubCounties(county);
                if (!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string nationality in nationalitiesArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Registration()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Registration> GetConstituencies(string subCounty)
        {
            var list = new List<Registration>();
            try
            {
                string nationalities = webportals.GetConstituencies(subCounty);
                if (!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string nationality in nationalitiesArr)
                    {
                        string[] responseArr = nationality.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Registration()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Applicant> GetMinimumRequirements(string applicationNo, string jobId, string refNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetMinimumRequirements(jobId);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        string code = responseArr[0];
                        string description = responseArr[1];
                        if (webportals.HasInsertedMinimumRequirement(applicationNo, refNo, code, jobId, description)) continue;
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Applicant> GetSubmittedMinimumRequirements(string jobId, string refNo)
        {
            var list = new List<Applicant>();
            try
            {
                string applicationNo = HttpContext.Current.Session["ApplicationNo"].ToString();
                string requirements = webportals.GetSubmittedMinimumRequirements(jobId, refNo, applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                            Answer = responseArr[2]
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

        public static List<Applicant> GetQualifications(string applicationNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetApplicantQualifications(applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Institution = responseArr[1],
                            DateFrom = DateTime.Parse(responseArr[2]),
                            DateTo = DateTime.Parse(responseArr[3]),
                            Course = responseArr[4],
                            SystemId = responseArr[5]
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

        public static List<Applicant> GetProfessionalQualifications(string applicationNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetProfessionalQualifications(applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Institution = responseArr[0],
                            Award = responseArr[1],
                            Specialization = responseArr[2],
                            Grade = responseArr[3],
                            DateFrom = DateTime.Parse(responseArr[4]),
                            DateTo = DateTime.Parse(responseArr[5]),
                            SystemId = responseArr[6]
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

        public static List<Applicant> GetProfessionalBodies(string applicationNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetProfessionalBodies(applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            ProffessionalBody = responseArr[0],
                            MembershipNo = responseArr[1],
                            MembershipType = responseArr[2],
                            DateOfRenewal = DateTime.Parse(responseArr[3]),
                            SystemId = responseArr[4]
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

        public static List<Applicant> GetNextOfKins(string applicationNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetNextOfKin(applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            NokName = responseArr[0],
                            NokAddress = responseArr[1],
                            NokTel = responseArr[2],
                            NokRelationship = responseArr[3],
                            SystemId = responseArr[4]
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

        public static List<Applicant> GetApplicantReferees(string applicationNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetApplicantReferees(applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            RefereeName = responseArr[0],
                            RefereeInstitution = responseArr[1],
                            RefereeDesignation = responseArr[2],
                            RefereeEmail = responseArr[3],
                            RefereePhone = responseArr[4],
                            SystemId = responseArr[5]
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

        public static List<Applicant> GetApplicantTrainings(string applicationNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetTrainingsAttended(applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Institution = responseArr[0],
                            Course = responseArr[1],
                            Duration = responseArr[2],
                            Year = Convert.ToInt32(responseArr[3]),
                            SystemId = responseArr[4]
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

        public static List<Applicant> GetEmploymentDetails(string applicationNo)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetEmploymentDetails(applicationNo);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Designation = responseArr[0],
                            JobGrade = responseArr[1],
                            Institution = responseArr[2],
                            DateFrom = Convert.ToDateTime(responseArr[3]),
                            DateTo = Convert.ToDateTime(responseArr[4]),
                            SystemId = responseArr[5]
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

        public static List<Applicant> GetAreasOfStudy()
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetAreaOfStudy();
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Applicant> GetAreasOfSpecialization(string areaOfStudy)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetAreaOfSpecialization(areaOfStudy);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Applicant> GetAwards(string areaOfSpecialization)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetAward(areaOfSpecialization);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Applicant> GetCourses(string award, string areaOfSpecialization)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetCourse(award, areaOfSpecialization);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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

        public static List<Applicant> GetGrades(string course)
        {
            var list = new List<Applicant>();
            try
            {
                string requirements = webportals.GetGrades(course);
                if (!string.IsNullOrEmpty(requirements))
                {
                    string[] requirementsArr = requirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string requirement in requirementsArr)
                    {
                        string[] responseArr = requirement.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Applicant()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
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