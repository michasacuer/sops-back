using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ProductIssue
    {
        [Key]
        public int    Id    { get; set; }
        public string Issue { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId                { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Product")]
        public int ProductId           { get; set; }
        public virtual Product Product { get; set; }
    }
}