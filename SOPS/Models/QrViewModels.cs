using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class QrViewModel
    {
        public int ProductId { get; set; }
        public int ExistingProductId { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Secret { get; set; }
    }
}