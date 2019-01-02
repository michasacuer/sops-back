using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class GetProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Kind { get; set; }
        public string AddressStreet { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressCity { get; set; }
        public string Email { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
    }
}