using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class RoleBindingModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public int OptionalCompanyId { get; set; }
    }
}