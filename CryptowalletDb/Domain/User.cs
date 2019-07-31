using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptowalletDb.Domain
{
    public class User
    {
        public User(){
            UserBankAccount = new List<UserBankAccount>();
        }
        
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual ICollection<UserBankAccount> UserBankAccount { get; set; }
    }
}
