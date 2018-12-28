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
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Product Product { get; set; }
    }
}