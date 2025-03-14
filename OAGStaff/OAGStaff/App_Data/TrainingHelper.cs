using OAGStaff.Models;
using OAGStaff.NAVWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAGStaff
{
    public class TrainingHelper
    {
        private static Staffportal webportals = Components.ObjNav;
        private static string[] strLimiters = new string[] { "::" };
        private static string[] strLimiters2 = new string[] { "[]" };

        public static List<Training> GetMyTrainingApplications(string username)
        {
            var list = new List<Training>();
            try
            {
                string trainings = webportals.GetMyTrainingApplications(username);
                if (!string.IsNullOrEmpty(trainings))
                {
                    string[] trainingsArr = trainings.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in trainingsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            DocumentNo = responseArr[0],
                            Date = Convert.ToDateTime(responseArr[1]),
                            StartDate = Convert.ToDateTime(responseArr[2]),
                            EndDate = Convert.ToDateTime(responseArr[3]),
                            CourseTitle = responseArr[4],
                            Status = responseArr[5]
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingNeeds(string username)
        {
            var list = new List<Training>();
            try
            {
                string needs = webportals.GetTrainingAssessmentNeeds(username);
                if(!string.IsNullOrEmpty(needs))
                {
                    string[] needsArr = needs.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in needsArr)
                    {
                        string[] responseArr = item.Split(strLimiters,StringSplitOptions.None);
                        Training training = new Training()
                        {
                            DocumentNo = responseArr[0],
                            Date = Convert.ToDateTime(responseArr[1]),
                            StaffName = responseArr[2],
                            Directorate = responseArr[3],
                            Submitted = responseArr[4]
                        };
                        list.Add(training);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetAcademicQualifications(string username)
        {
            var list = new List<Training>();
            try
            {
                string qualifications = webportals.GetHighestAcademicQualifications(username);
                if (!string.IsNullOrEmpty(qualifications))
                {
                    string[] qualificationsArr = qualifications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in qualificationsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                        };
                        list.Add(training);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetProffessionalQualifications()
        {
            var list = new List<Training>();
            try
            {
                string qualifications = webportals.GetProffessionalTrainingSetups();
                if (!string.IsNullOrEmpty(qualifications))
                {
                    string[] qualificationsArr = qualifications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in qualificationsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1],
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingProffessionalQualifications(string documentNo)
        {
            var list = new List<Training>();
            try
            {
                string qualifications = webportals.GetTrainingNeedsProffessionalQualifications(documentNo);
                if (!string.IsNullOrEmpty(qualifications))
                {
                    string[] qualificationsArr = qualifications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in qualificationsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            DocumentNo = responseArr[0],
                            No = responseArr[1],
                            Answer = responseArr[2]
                        };
                        list.Add(training);
                    }
                }
            }
            catch( Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetProffessionalCourses(string documentNo)
        {
            var list = new List<Training>();
            try
            {
                string proffessionalCourses = webportals.GetProffessionalCourses(documentNo);
                if(!string.IsNullOrEmpty(proffessionalCourses))
                {
                    string[] proffessionalCoursesArr = proffessionalCourses.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string item in proffessionalCoursesArr)
                    {
                        string[] responseArr = item.Split(strLimiters,StringSplitOptions.None);
                        Training training = new Training()
                        {
                            DocumentNo = responseArr[0],
                            CourseTitle = responseArr[1],
                            MonthYear = responseArr[2],
                            TrainingInstitution = responseArr[3],
                            Duration = responseArr[4],
                            No = responseArr[5]
                        };
                        list.Add(training);
                    }
                }
            }
            catch( Exception ex )
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingGaps(string documentNo)
        {
            var list = new List<Training>();
            try
            {
                string trainingGaps = webportals.GetTrainingGaps(documentNo);
                if (!string.IsNullOrEmpty(trainingGaps))
                {
                    string[] trainingGapsArr = trainingGaps.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in trainingGapsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            DocumentNo = responseArr[0],
                            No = responseArr[1],
                            Description = responseArr[2]
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingIntervensions(string documentNo)
        {
            var list = new List<Training>();
            try
            {
                string trainingIntervensions = webportals.GetTrainingIntervensions(documentNo);
                if (!string.IsNullOrEmpty(trainingIntervensions))
                {
                    string[] trainingIntervensionsArr = trainingIntervensions.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in trainingIntervensionsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            DocumentNo = responseArr[0],
                            CourseTitle = responseArr[1],
                            TrainingInstitution = responseArr[2],
                            Duration = responseArr[3],
                            MonthYear = responseArr[4],
                            EstimatedCost = Convert.ToDecimal(responseArr[5]),
                            No = responseArr[6]
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingPeriods()
        {
            var list = new List<Training>();
            try
            {
                string periods = webportals.GetTrainingPeriods();
                if(!string.IsNullOrEmpty(periods))
                {
                    string[] periodsArr = periods.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in periodsArr)
                    {
                        Training training = new Training()
                        {
                            Period = item
                        };
                        list.Add(training);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingCourses()
        {
            var list = new List<Training>();
            try
            {
                string trainingCourses = webportals.GetTrainingCourses();
                if (!string.IsNullOrEmpty(trainingCourses))
                {
                    string[] trainingCoursesArr = trainingCourses.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in trainingCoursesArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingVotes()
        {
            var list = new List<Training>();
            try
            {
                string trainingVotes = webportals.GetTrainingVotes();
                if (!string.IsNullOrEmpty(trainingVotes))
                {
                    string[] trainingVotesArr = trainingVotes.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in trainingVotesArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            Vote = responseArr[0],
                            VoteName = responseArr[1]
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingLines(string documentNo)
        {
            var list = new List<Training>();
            try
            {
                string trainingLines = webportals.GetTrainingApplicationLines(documentNo);
                if (!string.IsNullOrEmpty(trainingLines))
                {
                    string[] trainingLinesArr = trainingLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in trainingLinesArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            StaffNo = responseArr[0],
                            StaffName = responseArr[1],
                            CourseFee = Convert.ToDecimal(responseArr[2]),
                            DsaAmount = Convert.ToDecimal(responseArr[3]),
                            TransportCost = Convert.ToDecimal(responseArr[4]),
                            DocumentNo= responseArr[5]
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainingBtos(string username)
        {
            var list = new List<Training>();
            try
            {
                string trainingBtos = webportals.GetTrainingBtos(username);
                if (!string.IsNullOrEmpty(trainingBtos))
                {
                    string[] trainingBtosArr = trainingBtos.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in trainingBtosArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        Training training = new Training()
                        {
                            DocumentNo = responseArr[0],
                            StaffName = responseArr[1],
                            ApplicationNo = responseArr[2],
                            Date = Convert.ToDateTime(responseArr[3]),
                            Status = responseArr[4],
                        };
                        list.Add(training);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return list;
        }

        public static List<Training> GetTrainings(string username)
        {
            var list = new List<Training>();
            try
            {
                string trainingBtos = webportals.GetTrainingNos(username);
                if (!string.IsNullOrEmpty(trainingBtos))
                {
                    string[] trainingBtosArr = trainingBtos.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in trainingBtosArr)
                    {
                        Training training = new Training()
                        {
                            DocumentNo=item,
                        };
                        list.Add(training);
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