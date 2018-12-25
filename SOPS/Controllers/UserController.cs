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
using SOPS.Models;

namespace SOPS.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/User
        /*public IQueryable<ApplicationUser> GetApplicationUsers()
        {
            return db.ApplicationUsers;
        }*/

        // GET: api/User/Profile/5
        [Route("Profile")]
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult GetApplicationUserProfile(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var asEmployee = db.Employees.Find(id);
            Company employeeCompany = null;
            if(asEmployee != null)
            {
                employeeCompany = asEmployee.Company;
            }

            return Ok(new UserProfileViewModel()
            {
                Name = applicationUser.UserName,
                WatcherProducts = null,
                IsEmployee = asEmployee != null,
                Company = employeeCompany,
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}