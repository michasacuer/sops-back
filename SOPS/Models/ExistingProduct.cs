using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int ProductId { get; set; }

        [Required]
        virtual public Product Product { get; set; }
        virtual public QR QR { get; set; }
    }
}