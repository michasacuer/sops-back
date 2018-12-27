using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SOPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SOPS.ModelHelpers
{
    public static class UserHelper
    {
        public static string GetCurrentUserId()
        {
            var userManager = HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentId = HttpContext.Current.User.Identity.GetUserId();
            return currentId;
        }

        public static IList<string> GetCurrentUserRoles()
        {
            var userManager = HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentId = HttpContext.Current.User.Identity.GetUserId();
            return userManager.GetRolesAsync(currentId).Result;
        }

        public static bool IsCurrentUserEmployedInCompany(this ApplicationDbContext context, int companyId)
        {
            var userId = GetCurrentUserId();
            var asEmployee = context.Employees.Find(userId);
            return asEmployee != null && asEmployee.CompanyId == companyId;
        }

        public static bool IsCurrentUserInRole(string roleName)
        {
            return GetCurrentUserRoles().Contains(roleName);
        }

        public static bool IsCurrentUserEmployedInCompanyOrAdministrator(this ApplicationDbContext context, int companyId)
        {
            return context.IsCurrentUserEmployedInCompany(companyId) || IsCurrentUserInRole("Administrator");
        }

        public static ApplicationUser GetCurrentUser()
        {
            var userManager = HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentId = HttpContext.Current.User.Identity.GetUserId();
            return userManager.FindById(currentId);
        }
    }
}