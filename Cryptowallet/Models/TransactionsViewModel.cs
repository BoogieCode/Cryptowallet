using CryptowalletDb.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cryptowallet.Models
{
    public class TransactionsViewModel
    {
        [Range(1, 10000)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Range(1,10000)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public virtual ICollection<UserTransaction> Transactions { get; set; }

    }
}