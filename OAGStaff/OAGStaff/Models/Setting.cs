using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAGStaff.Models
{
    public class Setting
    {
        public string OldPassword { get; set; }
        public string Newpassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}