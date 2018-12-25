using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class QR
    {
        [Key]
        public int Version { get; set; }
        public byte[] Content { get; set; }

        [Required]
        [ForeignKey("ExistingProduct")]
        public int ExistingProductId { get; set; }
        public virtual ExistingProduct ExistingProduct { get; set; }

        public void UpdateQR()
        {
            var code = ExistingProduct.Product.Name + ";"
                + ExistingProduct.Product.Description + ";"
                + ExistingProduct.ExpirationDate.ToString("dd/MM/yyyy") + ";"
                + ExistingProduct.Product.Id + ";"
                + ExistingProduct.Id;

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var ms = new MemoryStream();
            qrCode.GetGraphic(10).Save(ms, ImageFormat.Png);
            Content = ms.ToArray();
            Version = 1;
        }
    }
}