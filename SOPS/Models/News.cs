using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class News
    {
        public string Header { get; set; }
        public DateTime EventDate { get; set; }
        public string IconName { get; set; }
        public string Content { get; set; }
    }
}