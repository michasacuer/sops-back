using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public string CountryOfOrigin { get; set; }

        [Column(TypeName = "money")]
        public decimal SuggestedPrice { get; set; }

        virtual public Company Company { get; set; }
        virtual public List<ProductRating> ProductRatings { get; set; }
        virtual public List<ExistingProduct> ExistingProducts { get; set; }
    }
}