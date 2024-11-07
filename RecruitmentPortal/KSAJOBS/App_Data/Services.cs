using KSAJOBS.NAVWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using KSAJOBS.ViewModels;
using ViewModels;
using System.Reflection;

namespace KSAJOBS
{
    public class Services
    {
        public static Recruitment webportals = Components.ObjNav;
        public static string[] strLimiters = new string[] { "::" };
        public static string[] strLimiters2 = new string[] { "[]" };
        public static List<AdvertisedJobs> GetAdvertisedJobs()
        {
            var advertisedJobs = new List<AdvertisedJobs>();
            try
            {
                string advertJobs = webportals.GetAdvertisedJobs();
                if (!string.IsNullOrEmpty(advertJobs))
                {
                    int counter = 0;
                    string[] advertJobsArr = advertJobs.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string aJob in advertJobsArr)
                    {
                        counter++;
                        string[] responseArr = aJob.Split(strLimiters, StringSplitOptions.None);
                        AdvertisedJobs job = new AdvertisedJobs()
                        {
                            Counter = counter,
                            JobRefNo = responseArr[0],
                            JobId = responseArr[1],
                            JobTitle = responseArr[2],
                            VacantPositions = Convert.ToDecimal(responseArr[3]),
                            RequiredPositions = Convert.ToDecimal(responseArr[4]),
                        };
                        advertisedJobs.Add(job);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return advertisedJobs;
        }

        public static List<JobRequirement> GetJobRequirements(string jobId)
        {
            List<JobRequirement> jobRequirements = new List<JobRequirement>();
            try
            {
                string jobReqs = webportals.JobRequirements(jobId);
                if (!string.IsNullOrEmpty(jobReqs))
                {
                    string[] jobReqsArr = jobReqs.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string aJob in jobReqsArr)
                    {
                        string[] responseArr = aJob.Split(strLimiters,StringSplitOptions.None);
                        JobRequirement requirement = new JobRequirement()
                        {
                            Requirements = responseArr[1],
                        };
                        jobRequirements.Add(requirement);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return jobRequirements;
        }

        public static List<JobResponsibility> GetJobResponsibilities(string jobId)
        {
            List<JobResponsibility> jobResponsibilities = new List<JobResponsibility>();
            try
            {
                string jobReqs = webportals.JobDescription(jobId);
                if (!string.IsNullOrEmpty(jobReqs))
                {
                    string[] jobReqsArr = jobReqs.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string aJob in jobReqsArr)
                    {
                        string[] responseArr = aJob.Split(strLimiters, StringSplitOptions.None);
                        JobResponsibility jobResponsibility = new JobResponsibility()
                        {
                            Responsibilities = responseArr[1],
                        };
                        jobResponsibilities.Add(jobResponsibility);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return jobResponsibilities;
        }

        public static string GetJobDescription(string jobId)
        {
            string jobDescription = string.Empty;
            try
            {
                jobDescription = webportals.GetJobTitle(jobId);
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return jobDescription;
        }

        public static List<Citizenship> GetCitizenships()
        {
            List<Citizenship> citizenships = new List<Citizenship>();
            try
            {
                string nationalities = webportals.GetNationalities();
                if(!string.IsNullOrEmpty(nationalities))
                {
                    string[] nationalitiesArr = nationalities.Split(strLimiters2,StringSplitOptions.RemoveEmptyEntries);
                    foreach(string str in nationalitiesArr)
                    {
                        string[] responseArr = str.Split(strLimiters,StringSplitOptions.None);
                        Citizenship citizenship = new Citizenship()
                        {
                            Code=responseArr[0],
                            Name=responseArr[1],
                        };
                        citizenships.Add(citizenship);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return citizenships;
        }

        public static List<MyApplication> GetMyApplications(string username)
        {
            var myApplications = new List<MyApplication>();
            try
            {
                string myApplics = webportals.GetMyApplications(username);
                if(!string.IsNullOrEmpty(myApplics))
                {
                    int counter = 0;
                    string[] myApplicsArr = myApplics.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in myApplicsArr)
                    {
                        counter++;
                        string[] responseArr = str.Split(strLimiters, StringSplitOptions.None);
                        MyApplication app = new MyApplication()
                        {
                            Counter = counter,
                            JobApplicationNo = responseArr[0],
                            JobId = responseArr[1],
                            JobRefNo = responseArr[2],
                            JobTitle = responseArr[3],
                            DateApplied = Convert.ToDateTime(responseArr[4]),
                            Status = responseArr[5]
                        };
                        myApplications.Add(app);
                    }
                }
            }
            catch (Exception ex) { ex.Data.Clear(); }
            return myApplications;
        }

        public static List<ApplicantQualification> GetApplicantQualifications(string applicationNo)
        {
            var applicantQualifications = new List<ApplicantQualification>();
            try
            {
                string applicantQuals = webportals.GetApplicantQualifications(applicationNo);
                if(!string.IsNullOrEmpty(applicantQuals))
                {
                    string[] applicantQualsArr = applicantQuals.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    int counter = 0;
                    foreach(string  applicantQual in applicantQualsArr)
                    {
                        counter++;
                        string[] responseArr = applicantQual.Split(strLimiters, StringSplitOptions.None);
                        ApplicantQualification qualification = new ApplicantQualification()
                        {
                            Counter = counter,
                            ApplitionNo = responseArr[0],
                            QualificationType = responseArr[1],
                            QualificationCode = responseArr[2],
                            QualificationDescription = responseArr[3],
                            Institution=responseArr[4],
                            StartDate=Convert.ToDateTime(responseArr[5]),
                            EndDate=Convert.ToDateTime(responseArr[6])
                        };
                        applicantQualifications.Add(qualification);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return applicantQualifications;
        }        

        public static List<ApplicantReferee> GetApplicantReferee(string applicationNo)
        {
            var applicantReferees = new List<ApplicantReferee>();
            try
            {
                string referees = webportals.GetApplicantReferees(applicationNo);
                if(!string.IsNullOrEmpty(referees))
                {
                    int counter = 0;
                    string[] refereesArr = referees.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string referee in refereesArr)
                    {
                        counter++;
                        string[] responseArr = referee.Split(strLimiters, StringSplitOptions.None);
                        ApplicantReferee app = new ApplicantReferee()
                        {
                            Counter = counter,
                            FullName = responseArr[0],
                            Institution = responseArr[1],
                            Designation = responseArr[2],
                            Email = responseArr[3],
                            Phone = responseArr[4]
                        };
                        applicantReferees.Add(app);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return applicantReferees;
        }

        public static List<ApplicantHobby> GetApplicantHobbies(string applicationNo)
        {
            var applicantHobbies = new List<ApplicantHobby>();
            try
            {
                string applicantHobs = webportals.GetApplicantHobbies(applicationNo);
                if(!string.IsNullOrEmpty(applicantHobs))
                {
                    int counter = 0;
                    string[] applicantHobsArr = applicantHobs.Split(strLimiters2,StringSplitOptions.RemoveEmptyEntries);
                    foreach(string applicantHobsStr in applicantHobsArr)
                    {
                        counter++;
                        string[] responseArr = applicantHobsStr.Split(strLimiters,StringSplitOptions.None);
                        ApplicantHobby hobby = new ApplicantHobby()
                        {
                            Counter = counter,
                            ApplicantNo = responseArr[0],
                            Hobbies = responseArr[1]
                        };
                        applicantHobbies.Add(hobby);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return applicantHobbies;
        }

        public static List<ApplicantAttachments> GetApplicantAttachments(string applicationNo)
        {
            var applicantAttachments = new List<ApplicantAttachments>();
            try
            {
                //connection = Components.getconnToNAV();
                //command = new SqlCommand()
                //{
                //    CommandText = "spGetDocumentAttachments",
                //    CommandType = CommandType.StoredProcedure,
                //    Connection = connection
                //};
                //command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                //command.Parameters.AddWithValue("@DocNo", "'" + applicationNo + "'");
                //reader = command.ExecuteReader();
                //if (reader.HasRows)
                //{
                //    int counter = 0;
                //    while(reader.Read())
                //    {
                //        counter++;
                //        ApplicantAttachments attachments = new ApplicantAttachments()
                //        {
                //            Counter = counter,
                //            Description = reader["Description"].ToString(),
                //            DocumentNo = reader["Document No"].ToString(),
                //            CreatedAt = Convert.ToDateTime(reader["$systemCreatedAt"].ToString()),
                //            SystemId = reader["$systemId"].ToString()
                //        };
                //        applicantAttachments.Add(attachments);
                //    }
                //}
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return applicantAttachments;
        }

        public static List<QualificationType> GetQualificationTypes(string jobId)
        {
            var qualificationTypes = new List<QualificationType>();
            try
            {
                string response = Components.ObjNav.GetJobQualificationType(jobId);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(new string[] {"[]"},StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in responseArr)
                    {
                        string[] qualificationArr = str.Split(new string[] { "::" }, StringSplitOptions.None);
                        QualificationType type = new QualificationType()
                        {
                            Code = qualificationArr[0]
                        };
                        qualificationTypes.Add(type);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return qualificationTypes;
        }

        public static List<QualificationCode> GetQualificationCodes(string jobId, string code)
        {
            var qualificationCode = new List<QualificationCode>();
            try
            {
                string qualificationCodes = Components.ObjNav.GetJobQualificationCodes(jobId, code);
                if (!string.IsNullOrEmpty(qualificationCodes))
                {
                    string[] qualificationCodesArr = qualificationCodes.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string str in qualificationCodesArr)
                    {
                        string[] responseArr = str.Split(new string[] { "::" }, StringSplitOptions.None);
                        QualificationCode code1 = new QualificationCode()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
                        };
                        qualificationCode.Add(code1);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return qualificationCode;
        }
    }
}