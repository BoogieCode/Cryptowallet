using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using CryptowalletDb.Domain;

namespace CryptowalletDb.Mappings
{
    public class UserBankAccountMap : EntityTypeConfiguration<UserBankAccount>
    {
        public UserBankAccountMap()
        {
            ToTable("UserBankAccounts").HasKey(t => t.AccountId);
            HasRequired(t => t.User)
                .WithMany(t => t.UserBankAccount)
                .HasForeignKey(t => t.UserId);

            Property(t => t.AccountId).HasColumnName("AccountId").IsRequired();
            Property(t => t.UserId).HasColumnName("UserId").IsRequired();
            Property(t => t.Currency).HasColumnName("Currency").IsRequired().HasMaxLength(3);
            Property(t => t.Amount).HasColumnName("Amount").IsRequired();
           
        }

    }
}
