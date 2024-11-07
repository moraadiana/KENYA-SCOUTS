using KSAJOBS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.Models
{
    public class Hobby
    {
        public string ApplicantHobby { get; set; }
        public List<ApplicantHobby> ApplicantHobbies { get; set; }
    }
}