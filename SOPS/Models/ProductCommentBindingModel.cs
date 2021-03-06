﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ProductCommentBindingModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}