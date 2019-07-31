using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using CryptowalletDb.Domain;

namespace CryptowalletDb.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("Users").HasKey(t => t.UserId);

            Property(t => t.UserId).HasColumnName("UserId").IsRequired();
            Property(t => t.Email).HasColumnName("Email").IsRequired().HasMaxLength(64);
            Property(t => t.Password).HasColumnName("Password").IsRequired().HasMaxLength(32);
            Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(32);


        }


    }
}
