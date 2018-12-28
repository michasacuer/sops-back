using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class Employee
    {
        [Key]
        [ForeignKey("User"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }
        public int CompanyId { get; set; }

        public virtual Company         Company { get; set; }
        public virtual ApplicationUser User    { get; set; }
    }
}