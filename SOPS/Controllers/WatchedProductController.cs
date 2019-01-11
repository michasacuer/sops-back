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

        // GET: api/WatchedProduct
        /// <summary>
        /// Zwraca produkty obserwowane przez aktualnego użytkownika [autoryzacja wymagana]
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IEnumerable<WatchedProduct> GetWatchedProduct()
        {
            var userId = UserHelper.GetCurrentUserId();
            var productsList = db.WatchedProducts.Include(wp=>wp.Product).Include(wp =>wp.Product.Company).Where(u => u.ApplicationUserId == userId).ToList();

            return productsList;
        }

        // GET: api/WatchedProduct/{id}
        /// <summary>
        /// Zwraca produkty obserwowane przez użytkownika o danym id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetWatchedProduct(string id)
        {
            var userId = id;
            if (userId == null)
            {
                return BadRequest();
            }

            if (db.Users.Find(userId) == null)
            {
                return BadRequest();
            }

            db.WatchedProducts.Include(wp => wp.Product).Load();

            var productsList = db.WatchedProducts.Include(wp => wp.Product).Where(wp => wp.ApplicationUserId == userId).ToList();

            return Ok(productsList);
            /*
            var userId = UserHelper.GetCurrentUserId();
            var productsList = id == null ?
                db.WatchedProducts.Where(u => u.ApplicationUserId == userId).ToList() :
                db.WatchedProducts.Where(u => u.ApplicationUserId == id).ToList();
            */
        }

        // GET api/WatchedProduct/Check
        /// <summary>
        /// Sprawdza czy produkt o danym id jest obserowany przez aktualnego użytkownika [autoryzacja wymagana]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Check")]
        public IHttpActionResult GetCheck(int id)
        {
            var userId = UserHelper.GetCurrentUserId();

            var product = db.Products.Find(id);

            if (product == null)
            {
                return BadRequest();
            }

            var isWatched = db.WatchedProducts.Any(wp => wp.ProductId == product.Id &&
                                                   wp.ApplicationUserId == userId);

            return Ok(new { isWatched });
        }

        // POST: api/WatchedProduct/5
        /// <summary>
        /// Dodaj produkt do obserwowanych aktualnego uzytkownika [autoryzacja wymagana]
        /// </summary>
        /// <param name="id">id produktu</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(WatchedProduct))]
        public IHttpActionResult PostWatchedProduct(int id)
        {
            // get product to add to user's watched products
            var product = db.Products.Find(id);
            var userId = UserHelper.GetCurrentUserId();

            if (userId == null || product == null)
                return NotFound();

            if (db.WatchedProducts.Find(userId, id) != null)
            {
                return BadRequest("user already wathes the product");
            }

            db.WatchedProducts.Add(new WatchedProduct { ProductId = id, ApplicationUserId = userId });
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/WatchedProduct/5
        /// <summary>
        /// Usun obserwowany produkt aktualnemu użytkownikowi [autoryzacja wymagana]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [ResponseType(typeof(WatchedProduct))]
        public IHttpActionResult DeleteWatchedProduct(int id)
        {
            var userId = UserHelper.GetCurrentUserId();

            if (userId == null)
                return NotFound();

            var product = db.WatchedProducts.SingleOrDefault(wp => wp.ApplicationUserId == userId && wp.ProductId == id);
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