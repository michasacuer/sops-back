using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SOPS;
using SOPS.Models;

namespace SOPS.Controllers
{
    public class ShortUrlController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ShortUrl/5
        /// <summary>
        /// daj docelowy link dla skroconego linku
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(ShortUrl))]
        public IHttpActionResult GetShortUrl(string id)
        {
            ShortUrl shortUrl = db.ShortUrls.Find(id);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return Ok(shortUrl);
        }

        // POST: api/ShortUrl
        /// <summary>
        /// daj skrocny link dla docleowego linku
        /// </summary>
        /// <param name="destinationUrl"></param>
        /// <returns></returns>
        [ResponseType(typeof(ShortUrl))]
        public IHttpActionResult PostShortUrl(string destinationUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var shortUrl = new ShortUrl()
            {
                DestinationUrl = destinationUrl,
                Added = DateTime.Now
            };
            shortUrl.GenerateShortUrl();
            db.ShortUrls.Add(shortUrl);
            db.SaveChanges();

            return Ok(shortUrl);
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