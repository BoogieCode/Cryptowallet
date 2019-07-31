using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cryptowallet.Models
{
    public class ProfilViewModel
    {

        [Required]
        [MaxLength(64)]
        [Display(Name ="User")]
        public string Username { get; set; }

        [Required]
        [MaxLength(64)]
        [Display(Name = "Email")]
        public string  Email { get; set; }
    }
}