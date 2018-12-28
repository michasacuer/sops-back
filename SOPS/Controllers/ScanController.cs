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
    public class ScanController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Scan
        /*public IQueryable<Scan> GetScans()
        {
            return db.Scans;
        }*/

        // GET: api/Scan/5
        /// <summary>
        /// daj skany dla uzytkownika (mozna tylko swoje, administor moze wszystkie)
        /// </summary>
        /// <param name="id">id uzytkownika</param>
        /// <returns></returns>
        [Authorize]
        [ResponseType(typeof(Scan))]
        public IHttpActionResult GetScans(string id) // user id
        {
            var currentUserId = UserHelper.GetCurrentUserId();
            if (currentUserId == null)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }          

            if(id != currentUserId && !UserHelper.IsCurrentUserInRole("Administrator"))
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }            

            return Ok(db.Scans.Where(s => s.UserId == id).ToList());
        }

        // POST: api/Scan
        /// <summary>
        /// dodaj skan
        /// </summary>
        /// <param name="scanBindingModel"></param>
        /// <returns></returns>
        [Authorize]
        public IHttpActionResult PostScan(ScanBindingModel scanBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = UserHelper.GetCurrentUserId();
            if(currentUserId == null)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            var scanningProduct = db.ExistingProducts.Find(scanBindingModel.ExistingProductId);
            if(scanBindingModel.ExistingProductSecret != scanningProduct.Secret)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var scan = new Scan()
            {
                UserId = currentUserId,
                Date = DateTime.Now,
                ExistingProductId = scanBindingModel.ExistingProductId,                
            };

            db.Scans.Add(scan);
            db.SaveChanges();

            return Ok(scan);
        }        

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