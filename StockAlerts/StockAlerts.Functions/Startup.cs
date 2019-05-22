using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockAlerts.Data;
using StockAlerts.Domain.Services;
using System;
using AutoMapper;
using StockAlerts.Data.Repositories;
using StockAlerts.Domain.Repositories;

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
            ConfigureAutoMapper(builder);
        }

        private void ConfigureDbContexts(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });
        }

        private void ConfigureServices(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IAlertDefinitionsService, AlertDefinitionsService>();
            builder.Services.AddScoped<IStocksService, StocksService>();

            builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
            builder.Services.AddScoped<IStocksRepository, StocksRepository>();
        }

        private void ConfigureAutoMapper(IFunctionsHostBuilder builder)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<DataModelMappingProfile>();
            });
            Mapper.AssertConfigurationIsValid();

            builder.Services.AddSingleton(Mapper.Instance);
        }
    }
}
