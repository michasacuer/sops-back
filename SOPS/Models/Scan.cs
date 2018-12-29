using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class Scan
    {
        [Key]
        public int      Id                { get; set; }
        public int      ExistingProductId { get; set; }
        public string   UserId            { get; set; }
        public DateTime Date              { get; set; }

        public virtual ExistingProduct ExistingProduct { get; set; }
        public virtual ApplicationUser User               { get; set; }
    }
}