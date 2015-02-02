# Currency-Converter
Euro Currency Converter (ECB Exchange Rates)

A simple class to convert currencies using the latest European Central Bank exchange rates.

https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml

Lazy singleton instance will attempt to fetch and parse current rates from ECB. 
Is Refresed propperty will be set to True if rates were fetched and parsed correctly.
Refresh is forced by invoking .Refresh(). Hook this to a timer to get fresh rates every (working) day.
