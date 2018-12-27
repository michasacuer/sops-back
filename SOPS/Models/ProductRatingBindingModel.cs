using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ProductRatingBindingModel
    {
        [Required]
        [Display(Name = "Rating")]
        [Range(0.0, 5.0, ErrorMessage ="Enter valid number")]
        public float Rating { get; set; }
    }
}
