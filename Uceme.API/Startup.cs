// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Calrom Limited">
// Copyright (c) Calrom Limited. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Uceme.Api
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Uceme.API.Services;
    using Uceme.API.Settings;
    using Uceme.Model.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;
            this.Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = this.Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.Configure<SwaggerSettings>(this.Configuration.GetSection("SwaggerSettings"));
            var smtpSettingsSection = this.Configuration.GetSection("EmailSettings");

            var appSettings = new AppSettings();
            appSettingsSection.Bind(appSettings);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(
                        this.Configuration.GetSection("LoginExpirationTimeout").Value));
                    options.Cookie.Expiration = TimeSpan.FromMinutes(Convert.ToDouble(
                        this.Configuration.GetSection("LoginExpirationTimeout").Value));
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                    options.Events.OnSignedIn = context =>
                    {
                        if ((this.Configuration.GetSection("ModifyCookieDomain").Value.ToLower() == "true") &&
                            (context.Request.Headers["Referer"].Count != 0))
                        {
                            options.Cookie.Domain = this.Configuration.GetSection("UseRefererForCookie").Value.ToLower() == "true" ?
                                context.Request.Headers["Referer"][0].Substring(
                                    context.Request.Headers["Referer"][0].IndexOf("//", StringComparison.CurrentCulture) + 2,
                                    context.Request.Headers["Referer"][0].Length - context.Request.Headers["Referer"][0].IndexOf("//", StringComparison.CurrentCulture) - 3).Split(':')[0]
                                : context.Request.Headers["Host"][0].Split(':')[0];

                            options.Cookie.HttpOnly = false;
                        }
                        else
                        {
                            options.Cookie.Domain = null;
                        }

                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization();

            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter());
            });
            ////.AddJsonOptions(options =>
            ////{
            ////    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ////    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ////})
            ////.AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            var connectionString = this.Configuration.GetConnectionString("DefaultConnection");

            services.AddTransient<IMedicoService, MedicoService>();
            ////services.AddTransient<IInterlineAgreementRepository>(sp =>
            ////    new InterlineAgreementRepository(connectionString));
            ////services.AddTransient<IPrivilegeAvailabilityRepository>(sp =>
            ////    new PrivilegeAvailabilityRepository(connectionString));
            ////services.AddTransient<IFlightService, FlightService>();
            ////services.AddTransient<IPrivilegeTypeRepository, PrivilegeTypeRepository>();
            ////services.AddTransient<IZonalFareTypeRepository, ZonalFareTypeRepository>();
            ////services.AddTransient<ICompanyDetailRepository, CompanyDetailRepository>();
            ////services.AddTransient<ITaxService, TaxService>();
            ////services.AddTransient<IPricingService, PricingService>();
            ////services.AddTransient<ITaxRepository, TaxRepository>();
            ////services.AddTransient<IPassengerTypeClassificationRepository, PassengerTypeClassificationRepository>();
            ////services.AddTransient<IPassengerTypeWorkingRepository, PassengerTypeWorkingRepository>();
            ////services.AddTransient<IStaffProfileService, StaffProfileService>();
            ////services.AddTransient<IPassengerTypeClassificationService, PassengerTypeClassificationService>();
            ////services.AddTransient<ICostingEntryGroupRepository, CostingEntryGroupRepository>();
            ////services.AddTransient<IFeeService>(provider => new FeeService(provider.GetRequiredService<IUnitOfWorkFactory<StaffTravelDbContext>>(), appSettings.RevenueStreamId));
            ////services.AddTransient<IContractService, ContractService>();

            
            ////var swaggerSettings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
            ////services.AddSwaggerGen(options =>
            ////{
            ////    options.SwaggerDoc(swaggerSettings.SwaggerVersion, new Info { Title = swaggerSettings.SwaggerApp, Version = swaggerSettings.SwaggerVersion });
            ////    options.AddSecurityDefinition(
            ////        "Bearer",
            ////        new ApiKeyScheme
            ////        {
            ////            In = "header",
            ////            Description = "Please enter JWT with Bearer into field",
            ////            Name = "Authorization",
            ////            Type = "apiKey",
            ////        });
            ////    options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            ////    {
            ////        { "Bearer", Enumerable.Empty<string>() },
            ////    });
            ////});

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            ////AutoMapper.Mapper.Initialize(x => x.AddProfile(new MappingProfile()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                var settings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(settings.SwaggerUri, settings.SwaggerUri);
                });
            }

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseCors("CorsPolicy");
            }

            app.UseAuthentication();

            var appSettings = new AppSettings();
            this.Configuration.GetSection("AppSettings").Bind(appSettings);
        }
    }
}