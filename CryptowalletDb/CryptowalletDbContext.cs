using CryptowalletDb.Domain;
using CryptowalletDb.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptowalletDb
{
    public class CryptowalletDbContext : DbContext
    {
        static CryptowalletDbContext()
        {
            Database.SetInitializer<CryptowalletDbContext>(null);
        }

        public CryptowalletDbContext()
            :base("Name = CryptowalletDbContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserBankAccount> UserBankAccounts { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserBankAccountMap());
            modelBuilder.Configurations.Add(new UserTransactionMap());
        }
    }
}
