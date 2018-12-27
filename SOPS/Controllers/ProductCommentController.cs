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
    public class ProductCommentController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string loggedUserId = UserHelper.GetCurrentUserId();

        // GET: api/ProductComment/id
        [HttpGet]
        public IQueryable<ProductComment> GetProductComment(int id)
        {
            //we could use '=>' here, idk if necessery
            return db.ProductComments.Where(pc => pc.ProductId == id);
        }

        // POST: api/ProductComments/id
        [Authorize]
        [HttpPost]
        [ResponseType(typeof(ProductComment))]
        public IHttpActionResult PostProductComment(int id, ProductCommentBindingModel commentFromBody)
        {
            if (!db.Products.Any(p => p.Id == id) || commentFromBody == null || loggedUserId == null)
                return NotFound();

            db.ProductComments.Add(new ProductComment
            {
                Comment = commentFromBody.Comment,
                ApplicationUserId = loggedUserId,
                ProductId = id
            });

            return Ok();
        }

        // DELETE: api/ProductComment/5
        [ResponseType(typeof(ProductComment))]
        [HttpDelete]
        public IHttpActionResult DeleteProductComment(int id)
        {
            ProductComment productComment = db.ProductComments.Find(id);
            if (productComment == null)
            {
                return NotFound();
            }

            db.ProductComments.Remove(productComment);
            db.SaveChanges();

            return Ok(productComment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductCommentExists(int id)
        {
            return db.ProductComments.Count(e => e.Id == id) > 0;
        }
    }
}