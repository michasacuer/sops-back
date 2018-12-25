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
        public int UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public ApplicationUser User { get; set; }
    }
}