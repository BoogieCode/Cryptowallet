using CryptowalletDb;
using CryptowalletDb.Domain;
using CryptoWalletExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cryptowallet.Controllers
{
    public class TotalController : Controller
    {
        // GET: Total
        [ChildActionOnly]
        public ActionResult _TotalPartial()
        {

            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                ExchangeService exchangeService = new ExchangeService();
                List<UserBankAccount> userBankAccounts = ctx.UserBankAccounts.Where(b => b.UserId == currentUser.UserId).ToList();
                List<CurrencyRate> rates = exchangeService.GetConversionRate(Currency.EUR, new Currency[] { Currency.EUR, Currency.ETH, Currency.LTC, Currency.BTC, Currency.EOS });
                decimal total = 0;
                foreach(var account in userBankAccounts)
                {
                    decimal value = account.Amount * decimal.Parse(rates.Where(a => a.Currency.ToString() == account.Currency).ToString());
                    total += value;
                }

            }

            return View();
        }
    
    }
}