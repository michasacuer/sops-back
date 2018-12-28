using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ExistingProductViewModel
    {
        public int ProductId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Secrete { get; set; }
    }
}