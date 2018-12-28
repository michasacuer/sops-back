using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ScanBindingModel
    {
        public int    ExistingProductId     { get; set; }
        public string ExistingProductSecret { get; set; }
    }
}