using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using SOPS.Attributes;
using SOPS.ModelHelpers;
using SOPS.Models;

namespace SOPS.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Product/search?str=kaarol
        /// <summary>
        /// wyszukiwanie na podstawie nazwy
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Route("search")]
        public IEnumerable<Product> GetSearch(string str)
        {
            return db.Products.Where(p => p.Name.Contains(str)).ToList();
        }

        /// <summary>
        /// daj wszystkie produktu
        /// raczej tylko dla administratora, niebezpieczne
        /// </summary>
        /// <returns></returns>
        // GET: api/Products
        public IEnumerable<Product> GetProducts()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            return db.Products.Include(p => p.ProductRatings).Include(p => p.ProductComments).ToList();
        }

        /// <summary>
        /// daj produkty dla firmy o podanym id
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetProducts(int companyId)
        {
            var company = db.Companies.Find(companyId);
            if (company == null)
            {
                return NotFound();
            }
            var companyProducts = db.Products.Where(p => p.CompanyId == companyId).Include(p => p.ProductRatings).Include(p => p.ProductComments).ToList();
            foreach(var product in companyProducts) // zamiast viewmodela
            {
                product.Company = null;
            }

            return Ok(companyProducts);
        }

        // GET: api/Products/5
        /// <summary>
        /// pobierz produkt o podanym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Include(p => p.ProductRatings).Include(p => p.ProductComments).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Products/Newest
        /// <summary>
        /// pobierz produkt który został dodany jako ostatni
        /// </summary>
        /// <returns></returns>
        [Route("Newest")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetNewest()
        {
            var newestProductDate = db.Products.Max(p => p.CreationDate);
            var newestProduct = db.Products.First(p => p.CreationDate == newestProductDate);

            return Ok(newestProduct);
        }

        // PUT: api/Products/5
        /// <summary>
        /// modyfikacja produktu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            var local = db.Set<Product>().Local.FirstOrDefault(f => f.Id == product.Id);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        /// <summary>
        /// dodaj produkt
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        // POST: api/Products
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var company = db.Companies.Find(product.CompanyId);
            if (company == null)
            {
                return BadRequest("company not found");
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        /// <summary>
        /// usun produkt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Products/5
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Include(p => p.ExistingProducts.Select(e => e.QR)).Include(p => p.ProductPicture).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}