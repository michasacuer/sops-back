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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: api/ShortUrl/5
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
                Url = RandomString(5),
                Added = DateTime.Now
            };
            db.ShortUrls.Add(shortUrl);

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