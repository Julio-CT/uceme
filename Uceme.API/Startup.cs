// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="JCT Software">
// Copyright (c) JCT Software. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Uceme.Api
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Uceme.API.Data;
    using Uceme.API.Data.Models;
    using Uceme.API.Options;
    using Uceme.API.Services;
    using Uceme.API.Settings;
    using Uceme.API.Utilities;

    public class Startup
    {
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
                    this.Configuration.GetConnectionString("UcemeConnection")));

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
                        this.Configuration.GetSection("LoginExpirationTimeout").Value));
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

            services.AddControllersWithViews();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter());
            });

            services.AddTransient<IMedicoService, MedicoService>();
            services.AddTransient<IFotosService, FotosService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderSettings>(Configuration);

            var swaggerSettings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.SwaggerVersion, new OpenApiInfo { Title = swaggerSettings.SwaggerApp, Version = swaggerSettings.SwaggerVersion });
            });

            ////AutoMapper.Mapper.Initialize(x => x.AddProfile(new MappingProfile()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var settings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(settings.SwaggerUri, settings.SwaggerApp);
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
                app.UseCors("CorsPolicy");
            }

            ////app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseAuthentication();

            var appSettings = new AppSettings();
            this.Configuration.GetSection("AppSettings").Bind(appSettings);
        }
    }
}