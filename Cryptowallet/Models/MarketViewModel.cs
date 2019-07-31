using CryptoWalletExchange;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cryptowallet.Models
{
    public class MarketViewModel
    {
        [Display(Name = "Currency")]
        public Currency Currency { get; set; }

        [Display(Name = "Exchange Rates")]
        public List<CurrencyRate> ExchangeRates { get; set; }
    }
}