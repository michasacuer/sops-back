using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace SOPS.Models
{
    public class QR
    {
        [Key]
        [ForeignKey("ExistingProduct"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExistingProductId { get; set; }
        public int Version { get; set; }
        public byte[] Content { get; set; }

        [Required]
        public virtual ExistingProduct ExistingProduct { get; set; }

        public static string Compress(string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public static string Decompress(string s)
        {
            var bytes = Convert.FromBase64String(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }

        public void UpdateQR()
        {
            /*var code = ExistingProduct.Product.Name + ";"
                + ExistingProduct.Product.Description + ";"
                + ExistingProduct.ExpirationDate.ToString("dd/MM/yyyy") + ";"
                + ExistingProduct.Product.Id + ";"
                + ExistingProduct.Id;*/

            var qrViewModel = new QrViewModel()
            {
                ProductId = ExistingProduct.ProductId,
                ExistingProductId = ExistingProduct.Id,
                ProductName = ExistingProduct.Product.Name,
                CompanyName = ExistingProduct.Product.Company.Name,
                Secret = ExistingProduct.Secret,
                CreationDate = ExistingProduct.Product.CreationDate,
            };

            var code = JsonConvert.SerializeObject(qrViewModel, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var ms = new MemoryStream();
            qrCode.GetGraphic(5).Save(ms, ImageFormat.Png);
            Content = ms.ToArray();
            Version = 1;
        }
    }
}