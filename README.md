# Currency-Converter
Euro Currency Converter (ECB Exchange Rates)

A simple class to convert currencies using the latest European Central Bank exchange rates.

https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml

Lazy singleton instance will attempt to fetch and parse current rates from ECB. 
`IsRefresed` propperty will be set to True if rates were fetched and parsed correctly.
Refresh is forced by invoking `Refresh()`. Hook this to a timer to get fresh rates every (working) day.

## Some examples
```c#
            CurrencyConverter cvt = CurrencyConverter.Instance;     // Singleton instance

            decimal euroValue = cvt.ConvertToEuro(100m, "USD");     // convert 100 USD to €
            decimal usdValue = cvt.ConvertFromEuro(100m, "USD");    // convert 100€ to USD

            // force refresh in case we have stale data
            if (DateTime.UtcNow - cvt.Time < new TimeSpan(24, 0, 0))
                cvt.Refresh();

            // was refresh successful?
            if (cvt.IsRefresed)
            {
                // do cool stuff
            }
            else
            { 
                // maybe the ECB server is down?
                // wait and try refresing later
            }

```
