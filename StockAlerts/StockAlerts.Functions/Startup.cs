using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockAlerts.Data;

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
        }

        private void ConfigureDbContexts(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
