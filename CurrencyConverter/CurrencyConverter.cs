using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Zgemba.Utils
{
    public sealed class CurrencyConverter   // lazy singleton
    {
        private Dictionary<string, decimal> rates;

        public DateTime Time { get; private set; }

        public bool IsRefresed { get; private set; }

        private static readonly Lazy<CurrencyConverter> lazy =
            new Lazy<CurrencyConverter>(() => new CurrencyConverter());

        public static CurrencyConverter Instance
        {
            get { return lazy.Value; }
        }

        private CurrencyConverter()
        {
            Refresh();
        }


        public void Refresh()
        {
            string response;
            try
            {
                WebRequest req = WebRequest.Create("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
                WebResponse res = req.GetResponse();
                Stream data = res.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    response = sr.ReadToEnd();
                }
                res.Close();

                XDocument doc = XDocument.Parse(response);
                XNamespace ns1 = doc.Root.Attributes().
                    Where(a => a.IsNamespaceDeclaration).
                    Where(a => a.Name.Namespace == XNamespace.None).
                    Select(a => a.Value).First();

                this.rates = (from e in doc.Root.Element(ns1 + "Cube").Element(ns1 + "Cube").Elements(ns1 + "Cube")
                              select new
                              {
                                  currency = e.Attribute("currency").Value,
                                  rate = e.Attribute("rate").Value
                              }).ToDictionary(e => e.currency, e => Decimal.Parse(e.rate, System.Globalization.CultureInfo.InvariantCulture));

                this.Time = DateTime.ParseExact(doc.Root.Element(ns1 + "Cube").Element(ns1 + "Cube").Attribute("time").Value,
                                                "yyyy-MM-dd",
                                                System.Globalization.CultureInfo.InvariantCulture);

                this.IsRefresed = true;
            }
            catch
            {
                this.IsRefresed = false;
                return;
            }
        }


        public Decimal ConvertToEuro(Decimal value, string currencyCode)
        {
            if (this.rates.ContainsKey(currencyCode))
            {
                return value / this.rates[currencyCode];
            }
            else
            {
                throw new ArgumentException("Unsupported currency code " + currencyCode);
            }
        }


        public Decimal ConvertFromEuro(Decimal value, string currencyCode)
        {
            if (this.rates.ContainsKey(currencyCode))
            {
                return value * this.rates[currencyCode];
            }
            else
            {
                throw new ArgumentException("Unsupported currency code" + currencyCode);
            }
        }
    }
}
