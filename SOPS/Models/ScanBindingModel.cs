using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ScanBindingModel
    {
        [Required]
        public int    ExistingProductId     { get; set; }
        [Required]
        public string ExistingProductSecret { get; set; }
        [Required]
        public double LocationLongitude { get; set; }
        [Required]
        public double LocationLatitude { get; set; }
    }
}