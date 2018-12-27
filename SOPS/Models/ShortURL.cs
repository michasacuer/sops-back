using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Models
{
    public class ShortUrl
    {
        [Key]
        public string Url { get; set; }
        public string DestinationUrl { get; set; }
        public DateTime Added { get; set; }
    }
}