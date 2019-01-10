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
    [RoutePrefix("api/ProductRating")]
    public class ProductRatingController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProductRating/id
        /// <summary>
        /// pobierz oceny dla produktu
        /// </summary>
        /// <param name="id">id produktu</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ProductRating> GetProductRating(int id)
        {
            //we could use '=>' here, idk if necessery
            return db.ProductRatings.Where(pc => pc.ProductId == id);
        }

        // GET: api/ProductRating/id
        /// <summary>
        /// pobierz oceny uzytkownika
        /// </summary>
        /// <param name="id">id produktu</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Profile")]
        public IQueryable<ProductRating> GetProductRating(string id)
        {
            return db.ProductRatings.Where(pr => pr.UserId == id);
        }


        // POST: api/ProductRating/id
        /// <summary>
        /// wstaw ocene produktu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rateFromBody"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult PostProductRating(int id, ProductRatingBindingModel rateFromBody)
        {
            if (!db.Products.Any(p => p.Id == id))
                return NotFound();

            var productRating = new ProductRating
            {
                Rating = rateFromBody.Rating,
                UserId = UserHelper.GetCurrentUserId(),
                ProductId = id,
                Added = DateTime.Now
            };

            db.ProductRatings.Add(productRating);
            db.SaveChanges();

            return Ok(productRating);
        }

        // GET: api/ProductRating/Avarage/id
        /// <summary>
        /// pobierz srednia ocen dla daego produktu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Avarage/{id:int}")]
        [HttpGet]
        [ResponseType(typeof(ProductRatingViewModel))]
        public IHttpActionResult GetAvarage(int id)
        {
            if (!db.Products.Any(p => p.Id == id))
                return NotFound();

            var response = new ProductRatingViewModel();

            response.ProductId = id;
            try
            {
                response.AvarageRating = db.ProductRatings.Where(p => p.ProductId == id).Average(p => p.Rating);
            }
            catch
            {
                response.AvarageRating = 0;
            }

            return Ok(response);               
        }

        //DELETE
        [Authorize]
        [Route("{userId}/{productid:int}")]
        [HttpDelete]
        [ResponseType(typeof(ProductRating))]
        public IHttpActionResult DeleteRating(string userId, int productId)
        {
            if (!(UserHelper.GetCurrentUserId() == userId))
                return NotFound();

            try
            {
                var rate = db.ProductRatings.First(pr => pr.UserId == userId && pr.ProductId == productId);
                db.ProductRatings.Remove(rate);
                db.SaveChanges();
            }
            catch
            {
                return NotFound();
            }

            return Ok();
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