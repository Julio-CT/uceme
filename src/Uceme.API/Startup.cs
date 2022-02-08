// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="JCT Software">
// Copyright (c) JCT Software. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Uceme.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Uceme.Foundation.Utilities;
    using Uceme.Library.Services;
    using Uceme.Model.Data;
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
            services.Configure<AuthMessageSenderSettings>(this.Configuration.GetSection("EmailSettings"));

            var appSettings = new AppSettings();
            appSettingsSection.Bind(appSettings);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("UcemeConnection")).EnableSensitiveDataLogging());

            services.AddControllers();

            services.AddCors(o =>
            {
                o.AddPolicy(this.relaxedPolicy, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });

                o.AddPolicy(this.strictPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                            .WithMethods("PUT", "DELETE", "GET", "POST");
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
            services.AddTransient<IHospitalService, HospitalService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IConfiguration>(this.Configuration);

            var swaggerSettings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.SwaggerVersion, new OpenApiInfo { Title = swaggerSettings.SwaggerApp, Version = swaggerSettings.SwaggerVersion });
            });
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
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseCors(this.relaxedPolicy);
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
