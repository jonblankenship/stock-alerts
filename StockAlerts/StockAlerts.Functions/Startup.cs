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
using System.Text;
using System.Threading.Tasks;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.IdentityModel.Tokens;
using StockAlerts.Domain.Authentication;
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
            ConfigureIdentity(builder);
            ConfigureServices(builder);
            ConfigureJwtAuthentication(builder);
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

        private void ConfigureIdentity(IFunctionsHostBuilder builder)
        {
            builder.Services.AddIdentityCore<IdentityUser>(o => 
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                    o.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        private void ConfigureServices(IFunctionsHostBuilder builder)
        {
            // Services
            builder.Services.AddScoped<IAlertDefinitionsService, AlertDefinitionsService>();
            builder.Services.AddScoped<IStocksService, StocksService>();
            builder.Services.AddScoped<IDataUpdateService, DataUpdateService>();
            builder.Services.AddScoped<IStockDataWebClient, IntrinioClient>();
            builder.Services.AddScoped<INotificationsService, NotificationsService>();
            builder.Services.AddScoped<IAccountsService, AccountsService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<IUserPreferencesService, UserPreferencesService>();

            // Repositories
            builder.Services.AddTransient<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
            builder.Services.AddTransient<IStocksRepository, StocksRepository>();
            builder.Services.AddTransient<IApiCallsRepository, ApiCallsRepository>();
            builder.Services.AddTransient<IAppUsersRepository, AppUsersRepository>();
            builder.Services.AddTransient<IUserPreferencesRepository, UserPreferencesRepository>();

            // Factories
            builder.Services.AddScoped<IQueueClientFactory, QueueClientFactory>();
            builder.Services.AddScoped<IAlertCriteriaSpecificationFactory, AlertCriteriaSpecificationFactory>();
            builder.Services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
            builder.Services.AddScoped<ITokenFactory, TokenFactory>();

            // Domain models
            builder.Services.AddTransient<AppUser, AppUser>();
            builder.Services.AddTransient<UserPreferences, UserPreferences>();
            builder.Services.AddTransient<Stock, Stock>();
            builder.Services.AddTransient<AlertDefinition, AlertDefinition>();
            builder.Services.AddTransient<AlertCriteria, AlertCriteria>();

            // Settings
            _settings = new Settings();
            _settings.Initialize(_configuration, _isDevelopment);
            builder.Services.AddSingleton<ISettings>(_settings);
            builder.Services.AddSingleton<IIntrinioSettings>(_configuration.GetSection("IntrinioSettings").Get<IntrinioSettings>());

            // Misc
            builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            builder.Services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
        }

        private void ConfigureJwtAuthentication(IFunctionsHostBuilder builder)
        {
            // Register the ConfigurationBuilder instance of AuthSettings
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.AuthSettings.SecretKey));

            // Configure JwtIssuerOptions
            _settings.JwtIssuerOptions.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _settings.JwtIssuerOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = _settings.JwtIssuerOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer("WebJobsAuthLevel", configureOptions =>
            {
                configureOptions.ClaimsIssuer = _settings.JwtIssuerOptions.Issuer;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = _settings.JwtIssuerOptions.Issuer;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            if (!context.Response.Headers.ContainsKey("Token-Expired"))
                                context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            // api user claim policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(CustomClaimIdentifiers.Rol, CustomClaims.ApiAccess));
            });
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
