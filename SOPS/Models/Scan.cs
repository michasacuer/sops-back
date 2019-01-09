using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class Scan
    {
        [Key]
        [ForeignKey("ExistingProduct")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 1)]
        public int ExistingProductId { get; set; }
        [Key]
        [ForeignKey("User")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 2)]
        public string UserId { get; set; }
        public DateTime Date { get; set; }

        public virtual ExistingProduct ExistingProduct { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}