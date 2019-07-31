using System.ComponentModel.DataAnnotations;

namespace Cryptowallet.Models
{
    public class WalletViewModel
    {
        [Range(1, 10000)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; }

        [Range(1, 10000)]
        [Display(Name = "Id")]
        public int AccountId { get; set; }
        
        [Display(Name = "Change in last 24h")]
        public decimal CustomAverage { get; set; }
    }
}