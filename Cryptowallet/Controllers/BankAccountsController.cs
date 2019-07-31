using Cryptowallet.Models;
using CryptowalletDb;
using CryptowalletDb.Domain;
using CryptoWalletExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Cryptowallet.Controllers
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        // GET: BankAccounts
        public ActionResult Index()
        {

            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                List<UserBankAccount> userBankAccounts = ctx.UserBankAccounts.Where(b => b.UserId == currentUser.UserId).ToList();

                List<BankAccountViewModel> accountViewModels = userBankAccounts.Select(a => new BankAccountViewModel
                {
                    AccountID = a.AccountId,
                    Amount = a.Amount,
                    Currency = a.Currency
                }).ToList();
                return View(accountViewModels);
            }
        }

        public ActionResult Wallet()
        {

            var exchangeService = new ExchangeService();
            var walletsList = new List<WalletViewModel>();
            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                IQueryable<UserBankAccount> userBankAccounts = from u in ctx.UserBankAccounts
                                                               where u.UserId == currentUser.UserId
                                                               select u;
                //List<Cryptowallet.Models.TransactionViewModel> exchange = Model.Where(t => t.FromUser == t.ToUser && t.FromCurrency != t.ToCurrency).ToList();
                //List<UserTransaction> userTransactions = userBankAccounts.Where(u => ).ToList();

                //Transaction transaction


                var bankAccounts = userBankAccounts.ToList();
                foreach (var account in userBankAccounts)
                {
                    Enum.TryParse(account.Currency, out Currency currentCurrency);

                    walletsList.Add(new WalletViewModel
                    {
                        AccountId = account.AccountId,
                        Amount = account.Amount,
                        Currency = account.Currency,
                        CustomAverage = exchangeService.GenerateCustomAverage(currentCurrency, Currency.EUR)
                    });
                }

                return View(walletsList);
            }

        }


        [HttpGet]
        public ActionResult Deposit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Deposit(DepositViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (CryptowalletDbContext ctx = new CryptowalletDbContext())
                {
                    User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                    UserBankAccount eurAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == "EUR" && a.UserId == currentUser.UserId);
                    if (eurAccount == null)
                    {
                        eurAccount = new UserBankAccount
                        {
                            Currency = "EUR",
                            UserId = currentUser.UserId,
                            Amount = 0
                        };

                        ctx.UserBankAccounts.Add(eurAccount);
                    }

                    eurAccount.Amount += viewModel.Amount;

                    ctx.SaveChanges();


                    UserTransaction userTransaction = new UserTransaction
                    {
                        Amount = viewModel.Amount,
                        FromAccountId = eurAccount.AccountId,
                        ToAccountId = eurAccount.AccountId,
                        CurrencyRate = 0,
                        TransactionDate = DateTime.Now
                    };

                    ctx.UserTransactions.Add(userTransaction);
                    ctx.SaveChanges();

                }
                return RedirectToAction("Wallet");

            }
            return View(viewModel);
        }


        [HttpGet]
        public ActionResult Transactions()
        {
            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                List<UserBankAccount> currentUserBankAccounts = ctx.UserBankAccounts.Where(u => u.UserId == currentUser.UserId).ToList();
                List<int> currentUserAccountIds = currentUserBankAccounts.Select(b => b.AccountId).ToList();

                List<UserTransaction> currentUserTransactions = ctx.UserTransactions.Where(t => currentUserAccountIds.Contains(t.ToAccountId)
                || currentUserAccountIds.Contains(t.FromAccountId)).ToList();

                List<TransactionViewModel> viewModel = currentUserTransactions.Select(a => new TransactionViewModel
                {
                    Amount = a.Amount,
                    dateTime = a.TransactionDate,
                    Rate = a.CurrencyRate,
                    FromUser = a.FromAccount.User.Email,
                    ToUser = a.ToAccount.User.Email,
                    FromCurrency = a.FromAccount.Currency,
                    ToCurrency = a.ToAccount.Currency
                }).ToList();
                return View(viewModel);
            }
        }
        public ActionResult MyTransactions()
        {
            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                UserBankAccount myAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.UserId == currentUser.UserId);

                IQueryable<UserTransaction> userTransactions = from u in ctx.UserTransactions
                                                               where u.FromAccountId == myAccount.AccountId || u.ToAccountId == myAccount.AccountId
                                                               select u;
                return View(userTransactions.ToList());

            }
        }

        /*     [HttpPost]
             public ActionResult Transactions(TransactionsViewModel viewModel)
             {
                 if (ModelState.IsValid)
                 {
                     using (CryptowalletDbContext ctx = new CryptowalletDbContext())
                     {
                         User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                         UserBankAccount FromEuroAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == "EUR" && a.UserId == currentUser.UserId);
                         UserBankAccount ToEuroAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.UserId == viewModel.Id);

                         if (FromEuroAccount.Amount >= viewModel.Amount && ctx.Users.FirstOrDefault(a => a.UserId == viewModel.Id) != null)
                         {
                             if (ToEuroAccount == null)
                             {
                                 ToEuroAccount = new UserBankAccount
                                 {
                                     Currency = "EUR",
                                     UserId = viewModel.Id,
                                     Amount = 0
                                 };

                                 ctx.UserBankAccounts.Add(ToEuroAccount);
                             }

                             ToEuroAccount.Amount += viewModel.Amount;
                             FromEuroAccount.Amount -= viewModel.Amount;
                             ctx.SaveChanges();


                             UserTransaction userTransaction = new UserTransaction
                             {
                                 Amount = viewModel.Amount,
                                 FromAccountId = FromEuroAccount.AccountId,
                                 ToAccountId = ToEuroAccount.AccountId,
                                 CurrencyRate = 0,
                                 TransactionDate = DateTime.Now
                             };

                             ctx.UserTransactions.Add(userTransaction);
                             ctx.SaveChanges();
                             return RedirectToAction("Index");
                         }
                         else
                         {
                             return View(viewModel);
                         }

                     }
                 }
                 return View(viewModel);
             }
     */
        [HttpGet]
        public ActionResult Send()
        {
            SendViewModel viewModel = new SendViewModel();
            SetupSendViewModel(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Send(SendViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (CryptowalletDbContext ctx = new CryptowalletDbContext())
                {
                    User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                    SetupSendViewModel(viewModel);


                    UserBankAccount fromAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.AccountId == viewModel.SenderAccountId && a.UserId == currentUser.UserId);
                    UserBankAccount toAccount = ctx.UserBankAccounts.AsNoTracking().FirstOrDefault(u => u.User.Email == viewModel.ReciverName && u.Currency == fromAccount.Currency);

                    if (viewModel.Amount > fromAccount.Amount)
                    {
                        ModelState.AddModelError("", "Insuficient funds");
                    }
                    else if (toAccount == null)
                    {
                        ModelState.AddModelError("", "Reciver don`t have an account in this currency or doesn`t exist");
                    }
                    else
                    {
                        fromAccount.Amount -= viewModel.Amount;
                        toAccount.Amount += viewModel.Amount;

                        ctx.SaveChanges();


                        UserTransaction userTransaction = new UserTransaction
                        {
                            Amount = viewModel.Amount,
                            FromAccountId = fromAccount.AccountId,
                            ToAccountId = toAccount.AccountId,
                            CurrencyRate = 0,
                            TransactionDate = DateTime.Now
                        };

                        ctx.UserTransactions.Add(userTransaction);
                        ctx.SaveChanges();
                        ViewBag.successMessage = "Transaction successful";

                        return View(viewModel);

                    }

                }
            }
            return View(viewModel);
        }



        private void SetupSendViewModel(SendViewModel viewModel)
        {
            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                List<BankAccountViewModel> accounts = ctx.UserBankAccounts.Where(a => a.UserId == currentUser.UserId).Select(u => new BankAccountViewModel
                {
                    AccountID = u.AccountId,
                    Amount = u.Amount,
                    Currency = u.Currency
                }).ToList();

                viewModel.SenderAccounts.AddRange(accounts.Select(a => new SelectListItem
                {
                    Value = a.AccountID.ToString(),
                    Text = a.Currency
                }));
            }
        }


        public ActionResult Profil()
        {
            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {

                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                ProfilViewModel profilViewModel = new ProfilViewModel();
                profilViewModel.Email = currentUser.Email;
                profilViewModel.Username = currentUser.Name;


                return View(profilViewModel);
            }
        }

        public ActionResult Exchange()
        {
            ExchangeViewModel viewModel = new ExchangeViewModel();
            SetupExchangeViewModel(viewModel);
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Exchange(ExchangeViewModel viewModel)
        {
            SetupExchangeViewModel(viewModel);
            CurrencyRate changeRate = new CurrencyRate();

            if (ModelState.IsValid)
            {
                using (CryptowalletDbContext ctx = new CryptowalletDbContext())
                {
                    User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                    ExchangeService exchangeService = new ExchangeService();
                    Enum.TryParse(viewModel.fromCurrency, out Currency fromCurrency);
                    List<CurrencyRate> rates = exchangeService.GetConversionRate(fromCurrency, new Currency[] { Currency.EUR, Currency.ETH, Currency.LTC, Currency.BTC, Currency.EOS });
                    //  List<CurrencyRate> rate =e.GetConversionRate(Currency.BTC, new Currency[] { Currency.EUR, Currency.USD, Currency.GBP });

                    changeRate = rates.FirstOrDefault(a => a.Currency.ToString() == viewModel.toCurrency);

                    UserBankAccount bankAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == viewModel.toCurrency && a.UserId == currentUser.UserId);
                    UserBankAccount fromBankAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == viewModel.fromCurrency && a.UserId == currentUser.UserId);
                    if(fromBankAccount.Amount<viewModel.Amount)
                    {
                        ModelState.AddModelError("", "Insuficient funds");
                        return View(viewModel);
                    }
                    if (bankAccount == null)
                    {
                        bankAccount = new UserBankAccount
                        {
                            Currency = viewModel.toCurrency,
                            UserId = currentUser.UserId,
                            Amount = 0,
                            
                        };

                        ctx.UserBankAccounts.Add(bankAccount);
                    }




                    bankAccount.Amount += viewModel.Amount * changeRate.Rate;
                    fromBankAccount.Amount -= viewModel.Amount;
                    ctx.SaveChanges();


                    UserTransaction userTransaction = new UserTransaction
                    {
                        Amount = viewModel.Amount,
                        FromAccountId = fromBankAccount.AccountId,
                        ToAccountId = bankAccount.AccountId,
                        CurrencyRate = changeRate.Rate,
                        TransactionDate = DateTime.Now
                    };

                    ctx.UserTransactions.Add(userTransaction);
                    ctx.SaveChanges();
                    ViewBag.successMessage = "Transaction successful";

                }
                return View(viewModel);

            }
            return View(viewModel);
        }

        private void SetupExchangeViewModel(ExchangeViewModel viewModel)
        {
            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {

                List<string> currencys = Enum.GetValues(typeof(Currency))
                         .Cast<Currency>()
                         .Select(v => v.ToString())
                         .ToList();

                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                List<BankAccountViewModel> accounts = ctx.UserBankAccounts.Where(a => a.UserId == currentUser.UserId).Select(u => new BankAccountViewModel
                {
                    AccountID = u.AccountId,
                    Amount = u.Amount,
                    Currency = u.Currency
                }).ToList();



                viewModel.FromCurrencyList.AddRange(accounts.Select(a => new SelectListItem
                {
                    Value=a.Currency,
                    Text = a.Currency
                }));
                viewModel.ToCurrencyList.AddRange(currencys.Select(a => new SelectListItem
                {
                    Value = a,
                    Text = a
                }));
                
            }
        }
        public decimal CalculateProfit()
        {
            decimal totalProfit=0;
            ExchangeService exchangeService = new ExchangeService();
            CurrencyRate changeRate = new CurrencyRate();

            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                List<CurrencyRate> rates = exchangeService.GetConversionRate(Currency.EUR, new Currency[] { Currency.EUR, Currency.ETH, Currency.LTC, Currency.BTC, Currency.EOS });
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                IQueryable<UserBankAccount> userBankAccounts = from u in ctx.UserBankAccounts
                                                               where u.UserId == currentUser.UserId && u.Currency== Currency.EUR.ToString()
                                                               select u;

                UserBankAccount eurAccount = userBankAccounts.FirstOrDefault();
                try
                {
                    List<UserTransaction> userTransactions = eurAccount.FromTransaction.Where(a => a.FromAccountId != a.ToAccountId && a.ToAccount.UserId == currentUser.UserId).ToList();

                    foreach (var tr in userTransactions)
                    {

                        changeRate = rates.FirstOrDefault(a => a.Currency.ToString() == tr.ToAccount.Currency);

                        totalProfit += tr.Amount * tr.CurrencyRate - tr.Amount * changeRate.Rate;
                    }
                }
                catch
                {
                    totalProfit = 0;
                }
            }
           
                return totalProfit;
        }
        public decimal ComputeTotalInEuro()
        {

            decimal totalInEuro = 0;
            var exchangeService = new ExchangeService();

            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);
                IQueryable<UserBankAccount> userBankAccounts = from u in ctx.UserBankAccounts
                                                               where u.UserId == currentUser.UserId
                                                               select u;

                var totalAccounts = userBankAccounts.ToList();

                CurrencyRate exchangeEurRate;
                foreach (var account in totalAccounts)
                {
                    if (Enum.TryParse(account.Currency, out Currency currentCurrency))
                    {
                        exchangeEurRate = exchangeService.GetConversionRate(currentCurrency, new Currency[] { Currency.EUR }).FirstOrDefault();
                        if (exchangeEurRate != null)
                            totalInEuro += account.Amount * exchangeEurRate.Rate;
                    }
                }

                return totalInEuro;
            }
        }
        [OverrideAuthorization]
        public ActionResult Market()
        {
            var exchangeService = new ExchangeService();
            var allCurrencies = Enum.GetValues(typeof(Currency));
            var result = new List<MarketViewModel>();

            foreach (Currency currency in allCurrencies)
            {
                var currencyConversions = exchangeService.GetConversionRate(currency, allCurrencies.Cast<Currency>().ToArray());
                result.Add(new MarketViewModel
                {
                    Currency = currency,
                    ExchangeRates = currencyConversions
                });
            }

            return View(result);
        }

        [HttpGet]
        public ActionResult MakeTransactions(int transactionType, Currency currency)
        {
            var exchangeModel = new ExchangeViewModel();
           
            switch (transactionType)
            {
                case 0:
                    ViewBag.Title = "Sell";
                    break;
                case 1:
                    ViewBag.Title = "Buy";
                    break;
            }

            exchangeModel.toCurrency = currency.ToString();


            return View(exchangeModel);
        }

        [HttpPost]
        public ActionResult MakeTransactions(ExchangeViewModel exchangeViewModel)
        {
            ViewBag.successMessage = "Transaction successful";
            switch (TempData["TransactionType"])
            {
                case "Buy":
                    return Buy(exchangeViewModel);
                case "Sell":
                    return Sell(exchangeViewModel);
                default:
                    return null;
            }
        }

        private ActionResult Buy(ExchangeViewModel exchangeViewModel)
        {
            CurrencyRate changeRate = new CurrencyRate();
            int transactionType;
            if (Enum.TryParse(exchangeViewModel.toCurrency, out Currency currentCurrency))
            {

                if (ModelState.IsValid)
                {
                    using (CryptowalletDbContext ctx = new CryptowalletDbContext())
                    {
                        User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                        ExchangeService exchangeService = new ExchangeService();
                        var eurBankAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == Currency.EUR.ToString() && a.UserId == currentUser.UserId);


                        //Enum.TryParse(exchangeViewModel.fromCurrency, out Currency fromCurrency);
                        //List<CurrencyRate> rates = exchangeService.GetConversionRate(fromCurrency, new Currency[] { Currency.EUR, Currency.ETH, Currency.LTC, Currency.BTC, Currency.EOS });



                        List<CurrencyRate> rates = exchangeService.GetConversionRate(currentCurrency, new Currency[] { Currency.EUR });

                        changeRate = rates.FirstOrDefault(a => a.Currency == Currency.EUR);

                        if (changeRate.Rate * exchangeViewModel.Amount > eurBankAccount.Amount)
                        {
                            ModelState.AddModelError("", "Insuficient funds");


                            //return RedirectToAction("MakeTransactions", new { transactionType = 1 });
                            switch (TempData["TransactionType"])
                            {
                                case "Buy":
                                    transactionType = 1;
                                    break;
                                case "Sell":
                                    transactionType = 0;
                                    break;
                                default:
                                    transactionType = 0;
                                    break;
                            }

                            return MakeTransactions(transactionType, currentCurrency);
                        }

                        UserBankAccount bankAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == exchangeViewModel.toCurrency && a.UserId == currentUser.UserId);
                        if (bankAccount == null)
                        {
                            bankAccount = new UserBankAccount
                            {
                                Currency = exchangeViewModel.toCurrency,
                                UserId = currentUser.UserId,
                                Amount = 0
                            };

                            ctx.UserBankAccounts.Add(bankAccount);
                        }




                        bankAccount.Amount += exchangeViewModel.Amount;
                        eurBankAccount.Amount -= exchangeViewModel.Amount * changeRate.Rate;
                        ctx.SaveChanges();


                        UserTransaction userTransaction = new UserTransaction
                        {
                            Amount = exchangeViewModel.Amount,
                            FromAccountId = eurBankAccount.AccountId,
                            ToAccountId = bankAccount.AccountId,
                            CurrencyRate = changeRate.Rate,
                            TransactionDate = DateTime.Now
                        };

                        ctx.UserTransactions.Add(userTransaction);
                        ctx.SaveChanges();

                    }
                    return View(exchangeViewModel);

                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Currency");
                switch (TempData["TransactionType"])
                {
                    case "Buy":
                        transactionType = 1;
                        break;
                    case "Sell":
                        transactionType = 0;
                        break;
                    default:
                        transactionType = 0;
                        break;
                }
                return MakeTransactions(transactionType, currentCurrency);
            }
            return RedirectToAction("Wallet");
        }


        private ActionResult Sell(ExchangeViewModel exchangeViewModel)
        {
            CurrencyRate changeRate = new CurrencyRate();
            int transactionType;
            if (Enum.TryParse(exchangeViewModel.toCurrency, out Currency currentCurrency))
            {

                if (ModelState.IsValid)
                {
                    using (CryptowalletDbContext ctx = new CryptowalletDbContext())
                    {
                        User currentUser = ctx.Users.AsNoTracking().FirstOrDefault(u => u.Email == User.Identity.Name);

                        ExchangeService exchangeService = new ExchangeService();
                        var sellingAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == currentCurrency.ToString() && a.UserId == currentUser.UserId);
                        
                        if(sellingAccount == null)
                        {
                            ModelState.AddModelError("", "Inexistent account");

                            switch (TempData["TransactionType"])
                            {
                                case "Buy":
                                    transactionType = 1;
                                    break;
                                case "Sell":
                                    transactionType = 0;
                                    break;
                                default:
                                    transactionType = 0;
                                    break;
                            }

                            return MakeTransactions(transactionType, currentCurrency);
                        }

                        List<CurrencyRate> rates = exchangeService.GetConversionRate(currentCurrency, new Currency[] { Currency.EUR });

                        changeRate = rates.FirstOrDefault(a => a.Currency == Currency.EUR);

                        if (exchangeViewModel.Amount > sellingAccount.Amount)
                        {
                            ModelState.AddModelError("", "Insuficient funds");


                            //return RedirectToAction("MakeTransactions", new { transactionType = 1 });
                            switch (TempData["TransactionType"])
                            {
                                case "Buy":
                                    transactionType = 1;
                                    break;
                                case "Sell":
                                    transactionType = 0;
                                    break;
                                default:
                                    transactionType = 0;
                                    break;
                            }

                            return MakeTransactions(transactionType, currentCurrency);
                        }

                        UserBankAccount euroBankAccount = ctx.UserBankAccounts.FirstOrDefault(a => a.Currency == Currency.EUR.ToString() && a.UserId == currentUser.UserId);
                        
                        euroBankAccount.Amount += exchangeViewModel.Amount * changeRate.Rate;
                        sellingAccount.Amount -= exchangeViewModel.Amount;

                        ctx.SaveChanges();


                        UserTransaction userTransaction = new UserTransaction
                        {
                            Amount = exchangeViewModel.Amount,
                            FromAccountId = sellingAccount.AccountId,
                            ToAccountId = euroBankAccount.AccountId,
                            CurrencyRate = changeRate.Rate,
                            TransactionDate = DateTime.Now
                        };

                        ctx.UserTransactions.Add(userTransaction);
                        ctx.SaveChanges();

                    }
                    return View(exchangeViewModel);

                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Currency");
                switch (TempData["TransactionType"])
                {
                    case "Buy":
                        transactionType = 1;
                        break;
                    case "Sell":
                        transactionType = 0;
                        break;
                    default:
                        transactionType = 0;
                        break;
                }
                return MakeTransactions(transactionType, currentCurrency);
            }
            return RedirectToAction("Wallet");
        }
    }
}