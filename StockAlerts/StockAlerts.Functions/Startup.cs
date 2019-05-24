using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockAlerts.Data;
using StockAlerts.Data.Repositories;
using StockAlerts.DataProviders.Intrinio;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Model;

[assembly: FunctionsStartup(typeof(StockAlerts.Functions.Startup))]
namespace StockAlerts.Functions
{
    public class Startup : FunctionsStartup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureDbContexts(builder);
            ConfigureServices(builder);
            ConfigureHttpClients(builder);
            ConfigureAutoMapper(builder);
        }

        private void ConfigureDbContexts(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }

        private void ConfigureServices(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IAlertDefinitionsService, AlertDefinitionsService>();
            builder.Services.AddScoped<IStocksService, StocksService>();
            builder.Services.AddScoped<IDataUpdateService, DataUpdateService>();
            builder.Services.AddScoped<IStockDataWebClient, IntrinioClient>();

            builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
            builder.Services.AddScoped<IStocksRepository, StocksRepository>();

            builder.Services.AddTransient<Stock, Stock>();
        }

        private void ConfigureAutoMapper(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            builder.Services.AddAutoMapper(
                cfg => cfg.ConstructServicesUsing(t => serviceProvider.GetService(t)),
                new List<Assembly> { Assembly.GetAssembly(typeof(DataModelMappingProfile))});
        }

        private void ConfigureHttpClients(IFunctionsHostBuilder builder)
        {
            var stockAlertsUserAgent = _configuration.GetValue<string>("StockAlertsUserAgent");

            if (stockAlertsUserAgent == null)
                throw new Exception("`StockAlertsUserAgent` must be set");

            builder.Services.AddHttpClient(HttpsClients.IntrinioApi, c =>
            {
                c.BaseAddress = new Uri("https://api-v2.intrinio.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", stockAlertsUserAgent);
            });

            var intrinioSettings = new IntrinioSettings()
            {
                ApiKey = _configuration.GetValue<string>("IntrinioApiKey")
            };
            builder.Services.AddScoped<IIntrinioSettings, IntrinioSettings>(x => intrinioSettings);

        }
    }
}
