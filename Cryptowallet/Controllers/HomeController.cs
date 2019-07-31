using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoWalletExchange;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Cryptowallet.Models;

namespace Cryptowallet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ExchangeService exchangeService = new ExchangeService();
            List<CurrencyRate> rates = exchangeService.GetConversionRate(Currency.EUR, new Currency[] { Currency.EUR, Currency.ETH, Currency.LTC, Currency.BTC, Currency.EOS });
          //  List<CurrencyRate> rate =e.GetConversionRate(Currency.BTC, new Currency[] { Currency.EUR, Currency.USD, Currency.GBP });

            List<CurrencyRateViewModel> viewModel = rates.Select(a => new CurrencyRateViewModel
            {
                Currency = a.Currency.ToString(),
                Rates = a.Rate
            }).ToList();

            return View(viewModel);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}