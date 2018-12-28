using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SOPS;
using SOPS.Models;
using SOPS.ModelHelpers;

namespace SOPS.Controllers
{
    [RoutePrefix("api/WatchedProduct")]
    public class WatchedProductController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/WatchedProduct/get?id=42b8ccb0-4911-458f-a066-36b057954157
        /// <summary>
        /// daj wszystkie obserwowane produkty dla aktualnego uzytkownika
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("get")]
        public IEnumerable<WatchedProduct> GetWatchedProduct(string id) => id == null ?
                db.WatchedProducts.Where(u => u.ApplicationUserId == UserHelper.GetCurrentUserId()).ToList() :
                db.WatchedProducts.Where(u => u.ApplicationUserId == id).ToList();



        // POST: api/WatchedProduct/5
        /// <summary>
        /// dodaj obserwowany produkt (dla aktualnego uzytkownika)
        /// </summary>
        /// <param name="id">id produktu</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(WatchedProduct))]
        public IHttpActionResult PostWatchedProduct(int id)
        {
            //get product's id to add to user's watched products
            var product = db.Products.Find(id);

            if (UserHelper.GetCurrentUserId() == null || product == null)
                return NotFound();

            db.WatchedProducts.Add(new WatchedProduct { ProductId = id, ApplicationUserId = UserHelper.GetCurrentUserId() });
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/WatchedProduct/5
        /// <summary>
        /// usun obserwowany produkt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [ResponseType(typeof(WatchedProduct))]
        public IHttpActionResult DeleteWatchedProduct(int id)
        {
            if (UserHelper.GetCurrentUserId() == null)
                return NotFound();

            var product = db.WatchedProducts.SingleOrDefault(u => u.ApplicationUserId == UserHelper.GetCurrentUserId() && u.ProductId == id);
            if (product == null)
                return NotFound();

            db.WatchedProducts.Remove(product);
            db.SaveChanges();

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

        //private bool WatchedProductExists(int id)
        //{
        //    return db.WatchedProducts.Count(e => e.Id == id) > 0;
        //}
    }
}