using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ApplicantAttachments
    {
        public int Counter { get; set; }
        public string Description { get; set; }
        public string DocumentNo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SystemId { get; set; }
    }
}