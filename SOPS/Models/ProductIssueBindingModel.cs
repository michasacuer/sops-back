using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ProductIssueBindingModel
    {
        [Required]
        [Display(Name = "Issue")]
        public string Issue { get; set; }
    }
}