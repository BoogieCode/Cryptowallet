using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cryptowallet.Models
{
    public class CurrencyRateViewModel
    {
        public string Currency { set; get; }
        public decimal Rates { set; get; }
    }
}