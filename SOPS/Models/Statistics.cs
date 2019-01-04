using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }
        public int LastMonthCompanyCount { get; set; }
        public int LastMonthProductCount { get; set; }
    }
}