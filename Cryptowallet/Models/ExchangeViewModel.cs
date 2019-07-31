using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cryptowallet.Models
{
    public class ExchangeViewModel
    {
        
        [Display(Name = "From")]
        public string fromCurrency { get; set; }


        [Required]
        [Display(Name = "To")]
        public string toCurrency { get; set; }

        [Required]
        [Range(0.00001,10000)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }


        public List<SelectListItem> FromCurrencyList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ToCurrencyList { get; set; } = new List<SelectListItem>();


    }
}