using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using SOPS.Models;

namespace SOPS.Controllers
{
    public class QRController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/QR/5
        /// <summary>
        /// wygeneruj kod qr dla existinigproduct
        /// </summary>
        /// <param name="id">existingproduct id</param>
        /// <returns></returns>
        public HttpResponseMessage GetQR(int id)
        {
            var existingProduct = db.ExistingProducts.Find(id);

            if(existingProduct == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Entry(existingProduct).Reference(e => e.QR).Load();

            var qr = existingProduct.QR;

            if(qr == null)
            {
                qr = new QR
                {
                    ExistingProductId = existingProduct.Id,
                    ExistingProduct = existingProduct
                };
                db.Entry(existingProduct).Reference(e => e.Product).Load();
                qr.UpdateQR();
                db.QRs.Add(qr);
                db.SaveChanges();
            }

            var response = Request.CreateResponse();
            response.Content = new StreamContent(new MemoryStream(qr.Content));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}