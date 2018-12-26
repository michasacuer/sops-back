using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using QRCoder;

namespace SOPS.Models
{
    public class ExistingProduct
    {
        [Key]
        public int Id { get; set; }
        public DateTime ExpirationDate { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        // public int QrId { get; set; }
        public virtual QR QR { get; set; }
    }
}