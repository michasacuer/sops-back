using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int ProductId { get; set; }

        virtual public ApplicationUser User { get; set; }
        virtual public Product Product { get; set; }
    }
}