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
        public int      Id             { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate   { get; set; }
        [Required]
        public int    ProductId        { get; set; }
        public string Secret           { get; set; }

        public virtual Product Product { get; set; }
        public virtual QR      QR      { get; set; }

        public void GenerateSecret()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Secret = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}