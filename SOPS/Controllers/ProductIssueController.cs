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
    public class ProductIssueController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProductIssue/id
        [HttpGet]
        public IQueryable<ProductIssue> GetProductIssues(int id)
        {
            return db.ProductIssues.Where(pi => pi.ProductId == id); ;
        }

        // POST: api/ProductIssue/5
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(ProductIssue))]
        public IHttpActionResult PostProductIssue(int id, ProductIssueBindingModel issueFromBody)
        {
            var userId = UserHelper.GetCurrentUserId();

            if (!db.Products.Any(p => p.Id == id) || issueFromBody == null || userId == null)
                return NotFound();

            db.ProductIssues.Add(new ProductIssue
            { 
                Issue = issueFromBody.Issue,
                ApplicationUserId = userId,
                ProductId = id
            });
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/ProductIssue/5
        [ResponseType(typeof(ProductIssue))]
        public IHttpActionResult DeleteProductIssue(int id)
        {
            ProductIssue productIssue = db.ProductIssues.Find(id);
            if (productIssue == null)
            {
                return NotFound();
            }

            db.ProductIssues.Remove(productIssue);
            db.SaveChanges();

            return Ok(productIssue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductIssueExists(int id)
        {
            return db.ProductIssues.Count(e => e.Id == id) > 0;
        }
    }
}