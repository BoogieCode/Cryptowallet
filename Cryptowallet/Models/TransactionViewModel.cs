using System;

namespace Cryptowallet.Models
{
    public class TransactionViewModel
    {
        public string FromUser { set; get; }
        public string ToUser { set; get; }
        public decimal Amount { set; get; }
        public decimal Rate { set; get; }
        public DateTime dateTime { set; get; }
        public string FromCurrency { set; get; }
        public string ToCurrency { set; get; }

    }
}