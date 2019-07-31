using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cryptowallet.Models
{
    public class DepositViewModel
    {
        [Range(1,10000)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

    }
}