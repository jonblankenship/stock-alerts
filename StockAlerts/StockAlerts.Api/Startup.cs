using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockAlerts.Api.Middleware;
using StockAlerts.Data;
using StockAlerts.Data.Repositories;
using StockAlerts.DataProviders.Intrinio;
using StockAlerts.Domain.Authentication;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Factories;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using StockAlerts.Domain.Settings;

namespace StockAlerts.Api
{
    public class Startup
    {
        private Settings _settings;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ConfigureDbContexts(services);
            ConfigureIdentity(services);
            ConfigureDependencies(services);
            ConfigureJwtAuthentication(services);
            ConfigureHttpClients(services);
            ConfigureAutoMapper(services);
        }

        private void ConfigureDbContexts(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    ApplicationDbContext.ConfigureStartupOptions(Environment.IsDevelopment(), Configuration, options);
                },
                ServiceLifetime.Transient);
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentityCore<IdentityUser>(o =>
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

        private void ConfigureDependencies(IServiceCollection services)
        {
            // Services
            services.AddScoped<IAlertDefinitionsService, AlertDefinitionsService>();
            services.AddScoped<IStocksService, StocksService>();
            services.AddScoped<IDataUpdateService, DataUpdateService>();
            services.AddScoped<IStockDataWebClient, IntrinioClient>();
            services.AddScoped<INotificationsService, NotificationsService>();
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IUserPreferencesService, UserPreferencesService>();

            // Repositories
            services.AddTransient<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
            services.AddTransient<IStocksRepository, StocksRepository>();
            services.AddTransient<IApiCallsRepository, ApiCallsRepository>();
            services.AddTransient<IAppUsersRepository, AppUsersRepository>();
            services.AddTransient<IUserPreferencesRepository, UserPreferencesRepository>();

            // Factories
            services.AddScoped<IQueueClientFactory, QueueClientFactory>();
            services.AddScoped<IAlertCriteriaSpecificationFactory, AlertCriteriaSpecificationFactory>();
            services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
            services.AddScoped<ITokenFactory, TokenFactory>();

            // Domain models
            services.AddTransient<AppUser, AppUser>();
            services.AddTransient<UserPreferences, UserPreferences>();
            services.AddTransient<Stock, Stock>();
            services.AddTransient<AlertDefinition, AlertDefinition>();
            services.AddTransient<AlertCriteria, AlertCriteria>();

            // Settings
            _settings = new Settings();
            _settings.Initialize(Configuration, Environment.IsDevelopment());
            services.AddSingleton<ISettings>(_settings);
            services.AddSingleton<IIntrinioSettings>(Configuration.GetSection("IntrinioSettings").Get<IntrinioSettings>());

            // Misc
            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
        }
        private void ConfigureJwtAuthentication(IServiceCollection services)
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(CustomClaimIdentifiers.Rol, CustomClaims.ApiAccess));
            });
        }

        private void ConfigureHttpClients(IServiceCollection services)
        {
            var stockAlertsUserAgent = _settings.AppSettings.StockAlertsUserAgent;

            if (stockAlertsUserAgent == null)
                throw new Exception("`StockAlertsUserAgent` must be set");

            services.AddHttpClient(Apis.IntrinioV1, c =>
            {
                c.BaseAddress = new Uri("https://api-v2.intrinio.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", stockAlertsUserAgent);
            });

            services.AddHttpClient(Apis.IntrinioV2, c =>
            {
                c.BaseAddress = new Uri("https://api.intrinio.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", stockAlertsUserAgent);
            });
        }
        private void ConfigureAutoMapper(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            services.AddAutoMapper(
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            // Tell the app to use authentication
            app.UseAuthentication();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseHttpsRedirection();
            app.UseMvc();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
