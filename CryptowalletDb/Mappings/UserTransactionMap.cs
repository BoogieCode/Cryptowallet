using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using CryptowalletDb.Domain;

namespace CryptowalletDb.Mappings
{
    public class UserTransactionMap : EntityTypeConfiguration<UserTransaction>
    {
        public UserTransactionMap()
        {
            ToTable("UsersTransactions").HasKey(t => t.TransactionsId);

            HasRequired(t => t.FromAccount)
                .WithMany(t => t.FromTransaction)
                .HasForeignKey(t => t.FromAccountId);

            HasRequired(t => t.ToAccount)
                .WithMany(t => t.ToTransaction)
                .HasForeignKey(t => t.ToAccountId);


            Property(t => t.TransactionsId).HasColumnName("TransactionsId").IsRequired();
            Property(t => t.FromAccountId).HasColumnName("FromAccountId").IsRequired();
            Property(t => t.ToAccountId).HasColumnName("ToAccountId").IsRequired();
            Property(t => t.Amount).HasColumnName("Amount").IsRequired().HasPrecision(20, 10);
            Property(t => t.CurrencyRate).HasColumnName("CurrencyRate").IsRequired().HasPrecision(20,10);
            Property(t => t.TransactionDate).HasColumnName("TransactionDate").IsRequired();


        }
    }
}
