using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class CompanyDeleteRequest
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
    }
}