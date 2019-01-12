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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SOPS.Attributes;
using SOPS.ModelHelpers;
using SOPS.Models;

namespace SOPS.Controllers
{
    [RoutePrefix("api/ExistingProduct")]
    public class ExistingProductController : ApiController
    {
        private ApplicationDbContext  db = new ApplicationDbContext();

        //GET: api/ExistingProduct/ForProduct?id={productId}
        /// <summary>
        /// daj wszystkie istniejące produkty dla produktu o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ForProduct")]
        [Authorize(Roles = "Employee, Administrator")]
        public IHttpActionResult GetExistingProductForProduct(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return BadRequest();
            }

            var existingProducts = db.ExistingProducts.Where(ep => ep.ProductId == product.Id).ToList();

            return Ok(existingProducts.ConvertAll(ep => new ExistingProductViewModel
            {
                Id = ep.Id,
                ProductId = ep.ProductId,
                CreationDate = ep.CreationDate,
                ExpirationDate = ep.ExpirationDate
            }).ToList());
        }

        // GET: api/ExistingProducts
        /// <summary>
        /// daj wszystkie istniejace produkty
        /// raczej do wywalenia - potencjalnie niebezpieczne
        /// </summary>
        /// <returns></returns>
        /*public IQueryable<ExistingProduct> GetExistingProducts()
        {
            return db.ExistingProducts;
        }

        // GET: api/ExistingProducts/5
        /// <summary>
        /// pobierz existingproduct o zadanym id
        /// raczej do wywalenia - pootencjalnie niebezpieczne
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        */

        /*
        // PUT: api/ExistingProducts/5
        /// <summary>
        /// modyfikacja istniejacego produktu
        /// raczej do wywalenia - niebezpieczne, potrzebna autoryzacja
        /// </summary>
        /// <param name="id"></param>
        /// <param name="existingProduct"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
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

            var temp = existingProduct;
            temp = db.ExistingProducts.Find(id);
            db.Entry(temp).Reference(e => e.Product).Load();

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(temp.Product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var local = db.Set<ExistingProduct>().Local.FirstOrDefault(f => f.Id == existingProduct.Id);
            if(local != null)
            {
                db.Entry(local).State = EntityState.Detached;
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
        */

        // POST: api/ExistingProducts
        /// <summary>
        /// dodaj existingproduct
        /// dodac viewmodel
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(ExistingProductViewModel))]
        public IHttpActionResult PostExistingProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = db.Products.Find(id);
            if(product == null)
            {
                return NotFound();
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var existingProduct = new ExistingProduct()
            {
                ProductId = id,
                CreationDate = DateTime.Now.Date,
                ExpirationDate = DateTime.Now.AddMonths(product.DefaultExpirationDateInMonths),
            };
            existingProduct.GenerateSecret();
            db.ExistingProducts.Add(existingProduct);
            db.SaveChanges();

            return Ok(new ExistingProductViewModel
            {
                Id = existingProduct.Id,
                ProductId = existingProduct.ProductId,
                CreationDate = existingProduct.CreationDate,
                ExpirationDate = existingProduct.ExpirationDate
            });
        }

        // DELETE: api/ExistingProducts/5
        /// <summary>
        /// usun existingproduct
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /*[Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(ExistingProduct))]
        public IHttpActionResult DeleteExistingProduct(int id)
        {
            ExistingProduct existingProduct = db.ExistingProducts.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            db.Entry(existingProduct).Reference(p => p.Product).Load();

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(existingProduct.Product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            db.ExistingProducts.Remove(existingProduct);
            db.SaveChanges();

            return Ok(existingProduct);
        }*/

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