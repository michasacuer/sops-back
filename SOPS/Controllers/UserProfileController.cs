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
using Microsoft.AspNet.Identity.Owin;
using SOPS.ModelHelpers;
using SOPS.Models;

namespace SOPS.Controllers
{
    [RoutePrefix("api/User")]
    public class UserProfileController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        // GET: api/User
        /*public IQueryable<ApplicationUser> GetApplicationUsers()
        {
            return db.ApplicationUsers;
        }*/

        // GET: api/User/Profile/5
        /// <summary>
        /// daj profil uzytkownika imie, nazwisko, numer, mail itd
        /// dodac obserwowane produkty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Profile")]
        [ResponseType(typeof(UserProfileViewModel))]
        public IHttpActionResult GetUserProfile(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var asEmployee = db.Employees.Find(id);
            Company employeeCompany = null;
            if (asEmployee != null)
            {
                employeeCompany = asEmployee.Company;
            }

            return Ok(new UserProfileViewModel()
            {
                Name = applicationUser.UserName,
                Surname = applicationUser.Surname,
                PhoneNumber = applicationUser.PhoneNumber,
                Email = applicationUser.Email,
                WatchedProducts = null,
                IsEmployee = asEmployee != null,
                Company = employeeCompany,
            });
        }

        // PUT: api/User/Profile/asdasd
        /// <summary>
        /// zmodyfikuj profil uzytkownika (oczywisciej est autoryzacja)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [Authorize]
        [Route("Profile")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserProfile(string id, UserProfileBindingModel userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserHelper.GetCurrentUser();
            user.Name = userProfile.Name;
            user.Surname = userProfile.Surname;
            user.Email = userProfile.Email;
            user.PhoneNumber = userProfile.PhoneNumber;
            UserManager.UpdateAsync(user).Wait();

            return StatusCode(HttpStatusCode.NoContent);
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