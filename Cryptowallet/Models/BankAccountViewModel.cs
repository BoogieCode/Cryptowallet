using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cryptowallet.Models
{
    public class BankAccountViewModel
    {
        public int AccountID { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

    }
}