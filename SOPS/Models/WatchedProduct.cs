using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class WatchedProduct
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId                { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Product")]
        public int ProductId           { get; set; }
        public virtual Product Product { get; set; }
    }
}