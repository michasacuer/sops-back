using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class UserProfileBindingModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}