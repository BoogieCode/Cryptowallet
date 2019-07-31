using System;

namespace CryptowalletDb.Domain
{
    public class UserTransaction
    {
        public int TransactionsId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrencyRate { get; set; }
        public DateTime TransactionDate { get; set; }

        public virtual UserBankAccount FromAccount { get; set; }
        public virtual UserBankAccount ToAccount { get; set; }

    }
}
