using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSAJOBS.ViewModels
{
    public class JobDetail
    {
        public List<JobRequirement> JobRequirements { get; set; }
        public List<JobResponsibility> JobResponsibilities { get; set; }
    }
}