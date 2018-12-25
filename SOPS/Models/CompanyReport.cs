using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class CompanyReport
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }

        [Required]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}