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
using SOPS.Attributes;
using SOPS.Models;

namespace SOPS.Controllers
{
    [AllowCrossSiteJson]
    public class ExistingProductController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ExistingProducts
        public IQueryable<ExistingProduct> GetExistingProducts()
        {
            return db.ExistingProducts;
        }

        // GET: api/ExistingProducts/5
        [ResponseType(typeof(ExistingProduct))]
        public IHttpActionResult GetExistingProduct(int id)
        {
            ExistingProduct existingProduct = db.ExistingProducts.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            return Ok(existingProduct);
        }

        // PUT: api/ExistingProducts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExistingProduct(int id, ExistingProduct existingProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != existingProduct.Id)
            {
                return BadRequest();
            }

            db.Entry(existingProduct).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExistingProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ExistingProducts
        [ResponseType(typeof(ExistingProduct))]
        public IHttpActionResult PostExistingProduct(ExistingProduct existingProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExistingProducts.Add(existingProduct);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = existingProduct.Id }, existingProduct);
        }

        // DELETE: api/ExistingProducts/5
        [ResponseType(typeof(ExistingProduct))]
        public IHttpActionResult DeleteExistingProduct(int id)
        {
            ExistingProduct existingProduct = db.ExistingProducts.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            db.ExistingProducts.Remove(existingProduct);
            db.SaveChanges();

            return Ok(existingProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExistingProductExists(int id)
        {
            return db.ExistingProducts.Count(e => e.Id == id) > 0;
        }
    }
}