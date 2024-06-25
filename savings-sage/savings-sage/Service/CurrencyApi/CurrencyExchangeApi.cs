using DotNetEnv;
using currencyapi;

namespace savings_sage.Service.CurrencyApi;

public class CurrencyExchangeApi
{
    private readonly string _apikey;
    private readonly ILogger<CurrencyExchangeApi> _logger;

    public CurrencyExchangeApi(ILogger<CurrencyExchangeApi> logger)
    {
        _logger = logger;
        Env.Load();
        _apikey = Environment.GetEnvironmentVariable("CURRENCY_API_KEY");
    }

    #region notes

    //https://github.com/everapihq/freecurrencyapi-dotnet
    public void WorkInProgress()
    {
        var fx = new Currencyapi(_apikey);
        //fx.Currencies("(EUR,USD,HUF)"); //currency codes
        //fx.Latest("USD", "(EUR,HUF)"); //from, to
        // {
        //     "data": {
        //         "AED": 3.67306,
        //         "AFN": 91.80254,
        //         "ALL": 108.22904,
        //         "AMD": 480.41659,
        //         "...": "150+ more currencies"
        //     }
        // }
        //fx.Convert("1000", "2024-06-25", "USD", "(EUR,HUF)"); //1000 USD to EUR and HUF on 2024.06.25.'s exchange rate
    }

    #endregion
}