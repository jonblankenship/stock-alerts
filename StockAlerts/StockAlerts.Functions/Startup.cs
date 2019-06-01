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
using AutoMapper.EquivalencyExpression;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Factories;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Settings;
using StockAlerts.Resources;

[assembly: FunctionsStartup(typeof(StockAlerts.Functions.Startup))]
namespace StockAlerts.Functions
{
    public class Startup : FunctionsStartup
    {
        private readonly bool _isDevelopment;
        private readonly IConfigurationRoot _configuration;
        private Settings _settings;

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (!_isDevelopment)
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient =
                    new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider
                            .KeyVaultTokenCallback));

                configurationBuilder.AddAzureKeyVault(
                    $"https://{Environment.GetEnvironmentVariable("Vault")}.vault.azure.net/", keyVaultClient,
                    new DefaultKeyVaultSecretManager());
            }

            _configuration = configurationBuilder.Build();
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
            builder.Services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    ApplicationDbContext.ConfigureStartupOptions(_isDevelopment, _configuration, options);
                }, 
                ServiceLifetime.Transient);
        }

        private void ConfigureServices(IFunctionsHostBuilder builder)
        {
            // Services
            builder.Services.AddScoped<IAlertDefinitionsService, AlertDefinitionsService>();
            builder.Services.AddScoped<IStocksService, StocksService>();
            builder.Services.AddScoped<IDataUpdateService, DataUpdateService>();
            builder.Services.AddScoped<IStockDataWebClient, IntrinioClient>();
            builder.Services.AddScoped<INotificationsService, NotificationsService>();

            // Repositories
            builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
            builder.Services.AddScoped<IStocksRepository, StocksRepository>();
            builder.Services.AddScoped<IApiCallsRepository, ApiCallsRepository>();
            builder.Services.AddScoped<IAppUsersRepository, AppUsersRepository>();

            // Factories
            builder.Services.AddScoped<IQueueClientFactory, QueueClientFactory>();
            builder.Services.AddScoped<IAlertCriteriaSpecificationFactory, AlertCriteriaSpecificationFactory>();

            // Domain models
            builder.Services.AddTransient<Stock, Stock>();
            builder.Services.AddTransient<AlertDefinition, AlertDefinition>();
            builder.Services.AddTransient<AppUser, AppUser>();
            builder.Services.AddTransient<AlertCriteria, AlertCriteria>();

            // Settings
            _settings = new Settings();
            _settings.Initialize(_configuration, _isDevelopment);
            builder.Services.AddSingleton<ISettings>(_settings);
            builder.Services.AddSingleton<IIntrinioSettings>(_configuration.GetSection("IntrinioSettings").Get<IntrinioSettings>());
        }

        private void ConfigureAutoMapper(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            builder.Services.AddAutoMapper(
                cfg =>
                {
                    cfg.ConstructServicesUsing(t => serviceProvider.GetService(t));
                    cfg.AddCollectionMappers();
                },
                new List<Assembly>
                {
                    Assembly.GetAssembly(typeof(DataModelMappingProfile)),
                    Assembly.GetAssembly(typeof(ResourceModelMappingProfile))
                });
        }

        private void ConfigureHttpClients(IFunctionsHostBuilder builder)
        {
            var stockAlertsUserAgent = _settings.AppSettings.StockAlertsUserAgent;

            if (stockAlertsUserAgent == null)
                throw new Exception("`StockAlertsUserAgent` must be set");

            builder.Services.AddHttpClient(Apis.Intrinio, c =>
            {
                c.BaseAddress = new Uri("https://api-v2.intrinio.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", stockAlertsUserAgent);
            });
        }
    }
}
