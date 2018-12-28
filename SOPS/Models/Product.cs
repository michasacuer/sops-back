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
        public int Id               { get; set; }
        public string Name          { get; set; }
        public string Barcode       { get; set; }
        public string Description   { get; set; }
        public DateTime CreationDate{ get; set; }

        [Required]
        public string CountryOfOrigin { get; set; }

        [Column(TypeName = "money")]
        public decimal SuggestedPrice { get; set; }

        [ForeignKey("Company")]
        public int             CompanyId { get; set; }
        public virtual Company Company   { get; set; }

        public virtual List<ProductRating>   ProductRatings   { get; set; }
        public virtual List<ProductComment>  ProductComments  { get; set; }
        public virtual List<ProductIssue>    ProductIssues    { get; set; }
        public virtual List<ExistingProduct> ExistingProducts { get; set; }
        public virtual List<WatchedProduct>  WatchedProducts  { get; set; }
    }
}