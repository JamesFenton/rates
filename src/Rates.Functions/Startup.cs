﻿using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rates.Functions.Services;
using Rates.Functions.WriteModel;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(Rates.Functions.Startup))]

namespace Rates.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var settings = new Settings
            {
                DatabaseConnectionString = Environment.GetEnvironmentVariable("RATES_DB_CONNECTIONSTRING"),
                CoinMarketCapApiKey = Environment.GetEnvironmentVariable("CMC_API_KEY"),
                OpenExchangeRatesApiKey = Environment.GetEnvironmentVariable("OPENEXCHANGERATES_APPID"),
                IexApiKey = Environment.GetEnvironmentVariable("IEX_TOKEN"),
            };

            services.AddSingleton(settings);
            services.AddSingleton<Database>();
            services.AddSingleton<RateSaver>();
            
            // services
            services.AddSingleton<CoinMarketCapService>();
            services.AddSingleton<IexService>();
            services.AddSingleton<OpenExchangeRatesService>();

        }
    }
}
