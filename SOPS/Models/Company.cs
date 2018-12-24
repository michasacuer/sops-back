using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Kind { get; set; }
        public string AddressStreet { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressCity { get; set; }
        public string Email { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }

        //virtual public List<Product> Products { get; set; }
        //virtual public List<CompanyReport> CompanyReports { get; set; }
        //virtual public List<Employee> Employees { get; set; }
    }
}