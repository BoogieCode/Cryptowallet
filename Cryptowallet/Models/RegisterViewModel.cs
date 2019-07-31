using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cryptowallet.Models
{
    public class RegisterViewModel
    {

        [Required]
        [MaxLength(64)]
        [MinLength(3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        [MinLength(3)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(64)]
        [MinLength(3)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [MaxLength(64)]
        [MinLength(3)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
