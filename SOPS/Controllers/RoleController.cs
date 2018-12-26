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

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // POST: api/Role
        [Authorize(Roles = "Administrator,Employee")]
        [ResponseType(typeof(RoleBindingModel))]
        public IHttpActionResult PostApplicationUser(RoleBindingModel roleBindingModel)
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
            else if (roleBindingModel.Role == "Employee" && userRoles.Any(r => r == "Administrator") 
                && userRoles.Any(r => r == "Employee"))
            {
                if(db.Companies.Find(roleBindingModel.OptionalCompanyId) == null)
                {
                    return BadRequest();
                }
                db.Employees.Add(new Employee
                {
                    UserId = roleBindingModel.UserId,
                    CompanyId = roleBindingModel.OptionalCompanyId
                });
            } 
            else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            UserManager.AddToRoleAsync(roleBindingModel.UserId, roleBindingModel.Role).Wait();

            return Ok();
        }
    }
}