using System.Collections.Generic;

namespace CryptowalletDb.Domain
{
    public class UserBankAccount
    {
        public UserBankAccount()
        {
            FromTransaction = new List<UserTransaction>();
            ToTransaction = new List<UserTransaction>();
        }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<UserTransaction> FromTransaction { get; set; }
        public virtual ICollection<UserTransaction> ToTransaction { get; set; }
    }
}
