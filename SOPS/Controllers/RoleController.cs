using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SOPS.Models;

namespace SOPS.Controllers
{
    public class RoleController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        public ApplicationRoleManager RoleManager => Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        // POST: api/Role
        /// <summary>
        /// nadaj role (uprawnienia uzytkownikowi): Employee, Administrator (moze miec kilka)
        /// </summary>
        /// <param name="roleBindingModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,Employee")]
        [ResponseType(typeof(RoleBindingModel))]
        public IHttpActionResult PostRole(RoleBindingModel roleBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!RoleManager.Roles.Any(r => r.Name == roleBindingModel.Role))
            {
                return BadRequest();
            }

            if (db.Users.Find(roleBindingModel.UserId) == null)
            {
                return BadRequest();
            }

            var userRoles = UserManager.GetRolesAsync(User.Identity.GetUserId()).Result;

            if (roleBindingModel.Role == "Administrator" && userRoles.Any(r => r == "Administrator"))
            {
                // ???
            }
            else if (roleBindingModel.Role == "Employee" && (userRoles.Any(r => r == "Administrator") 
                || userRoles.Any(r => r == "Employee")))
            {
                if(db.Companies.Find(roleBindingModel.OptionalCompanyId) == null)
                {
                    return BadRequest();
                }
                if(db.Employees.Find(roleBindingModel.UserId) != null)
                {
                    return BadRequest();
                }
                db.Employees.Add(new Employee
                {
                    UserId = roleBindingModel.UserId,
                    CompanyId = roleBindingModel.OptionalCompanyId,
                    JoinDate = DateTime.Now.Date
                });
                db.SaveChanges();
            } 
            else if (roleBindingModel.Role == "User" && (userRoles.Any(r => r == "Employee")))
            {
                db.Employees.Remove(db.Employees.Find(roleBindingModel.UserId));
                db.SaveChanges();
            }
            else
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }

            UserManager.AddToRoleAsync(roleBindingModel.UserId, roleBindingModel.Role).Wait();

            return Ok();
        }
    }
}