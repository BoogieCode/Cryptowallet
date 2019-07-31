using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cryptowallet.Models
{
    public class SendViewModel
    {
        //public SendViewModel()
        //{
        //    CurrancyList = new SelectList(Currency);
        //}
        [Required]
        [Display(Name = "From Account")]
        public int SenderAccountId { get; set; }

        [Required]
        [Display(Name = "Friend`s mail")]
        public string ReciverName { get; set; }

        [Range(1, 10000)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }


        public List<SelectListItem> SenderAccounts { get; set; } = new List<SelectListItem>();
        //public string Currency { get; set; }
        //public SelectList CurrancyList { get; set; }
    }
}