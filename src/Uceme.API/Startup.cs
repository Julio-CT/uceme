// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="JCT Software">
// Copyright (c) JCT Software. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Uceme.Api
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Uceme.API.Services;
    using Uceme.API.Utilities;
    using Uceme.Model.Data;
    using Uceme.Model.Models;
    using Uceme.Model.Settings;

    public class Startup
    {
        private readonly string relaxedPolicy = "RelaxedCorsPolicy";
    
        private readonly string strictPolicy = "StrictCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = this.Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            services.Configure<SwaggerSettings>(this.Configuration.GetSection("SwaggerSettings"));
            services.Configure<AuthMessageSenderSettings>(this.Configuration.GetSection("EmailSettings"));

            var appSettings = new AppSettings();
            appSettingsSection.Bind(appSettings);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("UcemeConnection")).EnableSensitiveDataLogging());

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(
                        this.Configuration.GetSection("LoginExpirationTimeout").Value,
                        CultureInfo.CurrentCulture));
                    ////options.Cookie.Expiration = TimeSpan.FromMinutes(Convert.ToDouble(
                    ////    this.Configuration.GetSection("LoginExpirationTimeout").Value));
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };

                    options.Events.OnSignedIn = context =>
                    {
                        if ((this.Configuration.GetSection("ModifyCookieDomain").Value.ToLower(CultureInfo.CurrentCulture) == "true") &&
                            (context.Request.Headers["Referer"].Count != 0))
                        {
                            options.Cookie.Domain = this.Configuration.GetSection("UseRefererForCookie").Value.ToLower(CultureInfo.CurrentCulture) == "true" ?
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
            services.AddControllers();

            services.AddCors(o => {
                o.AddPolicy(this.relaxedPolicy, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                o.AddPolicy(this.strictPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                            .WithMethods("PUT", "DELETE", "GET");
                });
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter());
            });

            services.AddTransient<IMedicoService, MedicoService>();
            services.AddTransient<IFotosService, FotosService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IConfiguration>(this.Configuration);
            services.Configure<AuthMessageSenderSettings>(this.Configuration);

            var swaggerSettings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.SwaggerVersion, new OpenApiInfo { Title = swaggerSettings.SwaggerApp, Version = swaggerSettings.SwaggerVersion });
            });

            ////AutoMapper.Mapper.Initialize(x => x.AddProfile(new MappingProfile()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dataContext)
        {
            if (dataContext != null)
            {
                try
                {
                    // migrate any database changes on startup (includes initial db creation)
                    dataContext.Database.Migrate();
                }
                catch (SqlException)
                {
                    // no problem
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var settings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(settings.SwaggerUri.ToString(), settings.SwaggerApp);
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseCors(this.relaxedPolicy);
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var appSettings = new AppSettings();
            this.Configuration.GetSection("AppSettings").Bind(appSettings);
        }
    }
}