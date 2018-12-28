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
    [AllowCrossSiteJson]
    public class ExistingProductController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationRoleManager RoleManager => Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
        public ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        // GET: api/ExistingProducts
        /// <summary>
        /// daj wszystkie istniejace produkty
        /// raczej do wywalenia - potencjalnie niebezpieczne
        /// </summary>
        /// <returns></returns>
        public IQueryable<ExistingProduct> GetExistingProducts()
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

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(existingProduct.Product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
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
        /// <summary>
        /// dodaj existingproduct
        /// dodac viewmodel
        /// </summary>
        /// <param name="existingProduct"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(ExistingProductViewModel))]
        public IHttpActionResult PostExistingProduct(ExistingProduct existingProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(existingProduct.Product.CompanyId))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            existingProduct.CreationDate = DateTime.Now;
            existingProduct.GenerateSecret();
            db.ExistingProducts.Add(existingProduct);
            db.SaveChanges();

            return Ok(new ExistingProductViewModel
            {
                ProductId = existingProduct.ProductId,
                CreationDate = existingProduct.CreationDate,
                Secrete = existingProduct.Secret              
            });
        }

        // DELETE: api/ExistingProducts/5
        /// <summary>
        /// usun existingproduct
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
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