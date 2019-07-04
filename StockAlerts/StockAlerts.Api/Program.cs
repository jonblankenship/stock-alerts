using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Logging;

namespace StockAlerts.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("local.settings.json", true, true);

                    var buildConfig = config.Build();
                    if (buildConfig["ASPNETCORE_ENVIRONMENT"] != "Development")
                    {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient =
                            new KeyVaultClient(
                                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider
                                    .KeyVaultTokenCallback));

                        config.AddAzureKeyVault(
                            $"https://{buildConfig["Vault"]}.vault.azure.net/", keyVaultClient,
                            new DefaultKeyVaultSecretManager());
                    }
                })
                .UseStartup<Startup>();
    }
}
