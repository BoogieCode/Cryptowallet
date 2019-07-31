using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cryptowallet.Models
{
    public class LoginViewModel
    {

        [Required]
        [MaxLength(64)]
        [Display(Name ="Email")]
        public string Username { get; set; }

        [Required]
        [MaxLength(64)]
        [Display(Name = "Password")]
        public string  Password { get; set; }
    }
}