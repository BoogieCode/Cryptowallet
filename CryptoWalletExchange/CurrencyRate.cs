using System.ComponentModel.DataAnnotations;

namespace CryptoWalletExchange
{
    public class CurrencyRate
    {
        public Currency Currency { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        public decimal Rate { get; set; }
    }
}
