using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ScanViewModel
    {
        // Watched product
        public bool IsWatched { get; set; }

        // Scan
        public DateTime ScanDate { get; set; }

        // Existing product
        public int ExistingProductId { get; set; }
        public DateTime ExistingProductExpirationDate { get; set; }
        public DateTime ExistingProductCreationDate { get; set; }

        // Product
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBarcode { get; set; }
        public string ProductDescription { get; set; }
        public DateTime ProductCreationDate { get; set; }
        public int ProductDefaultExpirationDateInMonths { get; set; }
        public string ProductCountryOfOrigin { get; set; }
        [Column(TypeName = "money")]
        public decimal ProductSuggestedPrice { get; set; }

        // Company
        public string CompanyName { get; set; }
        public string CompanyAddressStreet { get; set; }
        public string CompanyAddressZipCode { get; set; }
        public string CompanyAddressCity { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyNIP { get; set; }
        public string CompanyREGON { get; set; }
        public DateTime CompanyJoinDate { get; set; }




    }
}