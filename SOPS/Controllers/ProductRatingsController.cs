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
using SOPS.ModelHelpers;
using SOPS.Models;

namespace SOPS.Controllers
{
    public class ProductRatingsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string loggedUserId = UserHelper.GetCurrentUserId();

        // GET: api/ProductRatings/id
        [HttpGet]
        public IQueryable<ProductRating> GetProductRatings(int id)
        {
            //we could use '=>' here, idk if necessery
            return db.ProductRatings.Where(pc => pc.ProductId == id);
        }

        // POST: api/ProductRatings/id
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(ProductRating))]
        public IHttpActionResult PostProductRating(int id, [FromBody] int rate)
        {
            if (!db.Products.Any(p => p.Id == id) || loggedUserId == null)
                return NotFound();

            db.ProductRatings.Add(new ProductRating
            {
                Rating = rate,
                UserId = loggedUserId,
                ProductId = id
            });

            return Ok();
        }

        // GET: api/ProductRatings/Avarage/id
        [Route("Avarage/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(ProductRatingViewModel))]
        public IHttpActionResult GetAvarage(int id)
        {
            if (!db.Products.Any(p => p.Id == id))
                return NotFound();

            return Ok(new ProductRatingViewModel
            {
                ProductId = id,
                AvarageRating = db.ProductRatings.Where(p => p.ProductId == id).Average(p => p.Rating)
            });               
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool ProductRatingExists(int id)
        //{
        //    return db.ProductRatings.Count(e => e.Id == id) > 0;
        //}
    }
}