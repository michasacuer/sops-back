using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ProductRating
    {
        [Key]
        public int Id { get; set; }
        public float Rating { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual List<Product> Product { get; set; }
    }
}