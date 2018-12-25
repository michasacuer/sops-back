using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class UserProfileViewModel
    {
        public string Name { get; set; }
        public bool IsEmployee { get; set; }
        public List<Product> WatcherProducts { get; set; }
        public Company Company { get; set; }
    }
}