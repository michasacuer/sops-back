﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ProductComment
    {
        [Key]
        public int      Id      { get; set; }
        public string   Comment { get; set; }
        public DateTime Date    { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId                { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Product")]
        public int ProductId           { get; set; }
        public virtual Product Product { get; set; }
    }
}