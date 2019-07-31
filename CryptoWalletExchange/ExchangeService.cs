using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWalletExchange
{
    public class ExchangeService
    {
        private const string BASE_URL = "https://min-api.cryptocompare.com";


        public List<CurrencyRate> GetConversionRate(Currency from, Currency[] to)
        {


            if (to == null || to.Length == 0)
            {
                throw new ArgumentException("to");
            }

            string url = $"{BASE_URL}/data/price?fsym={from}&tsyms={string.Join(",", to)}";
            //https://min-api.cryptocompare.com/data/price?fsym=BTC&tsyms=USD,JPY,EUR
            //  url = "https://min-api.cryptocompare.com/data/price?fsym=BTC&tsym=EUR,USD";
            //url = "https://min-api.cryptocompare.com/data/price?fsym=BTC&tsyms=USD,JPY,EUR";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";

            List<CurrencyRate> returnValues = new List<CurrencyRate>();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string context = reader.ReadToEnd();

                        Dictionary<string, decimal> convertionRates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(context);

                        foreach (KeyValuePair<string, decimal> rate in convertionRates)
                        {
                            returnValues.Add(new CurrencyRate
                            {
                                Rate = rate.Value,
                                Currency = (Currency)Enum.Parse(typeof(Currency), rate.Key)
                            });
                        }
                        return returnValues;
                    }
                }
            }

        }

        public decimal GenerateCustomAverage(Currency from, Currency to)
        {
            if (from == to)
            {
                return 1;
            }

            string url = $"{BASE_URL}/data/generateAvg?fsym={from}&tsym={to}&e=Coinbase";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string context = reader.ReadToEnd();
                        var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(context);
                        var deserializedDeserializedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(deserializedData["RAW"].ToString());
                        string customAverage = deserializedDeserializedData["CHANGEPCT24HOUR"];
                        decimal.TryParse(customAverage, out decimal result);
                        return decimal.Round(result, 2);
                    }
                }
            }
        }
    }
}
