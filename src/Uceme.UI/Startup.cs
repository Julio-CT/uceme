namespace Uceme.UI;

using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
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

    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        IConfigurationSection appSettingsSection = this.configuration.GetSection("AppSettings");
        services.Configure<AuthMessageSenderSettings>(this.configuration.GetSection("EmailSettings"));

        services.Configure<AppSettings>(appSettingsSection);
        string? ucemeConnection = this.configuration.GetConnectionString("UcemeConnection");

        if (ucemeConnection == null)
        {
            throw new InvalidDataException("missing UcemeConnection settings");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ucemeConnection));

        SetupIdentity(services);
        this.SetupCors(services);

        services.AddSingleton(this.configuration);

        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ISmtpClient, SmtpClientWrapper>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

        // In production, the React files will be served from this directory
        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
        });

        SwaggerSettings? swaggerSettings = this.configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
        if (swaggerSettings == null)
        {
            throw new InvalidDataException("missing swaggerSettings settings");
        }

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerSettings?.SwaggerVersion, new OpenApiInfo { Title = swaggerSettings?.SwaggerApp, Version = swaggerSettings?.SwaggerVersion });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();

            SwaggerSettings? swaggerSettings = this.configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
            if (swaggerSettings == null)
            {
                throw new InvalidDataException("missing Swagger settings");
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerSettings.SwaggerUri?.ToString(), swaggerSettings.SwaggerApp);
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        CorsSettings? corsSettings = this.configuration.GetSection("CorsSettings").Get<CorsSettings>();
        if (corsSettings == null)
        {
            throw new InvalidDataException("missing corsSettings settings");
        }

        _ = app.UseCors(corsSettings.UseStrictPolicy ?
            this.strictPolicy
            : this.relaxedPolicy);

        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
    }

    private static void SetupIdentity(IServiceCollection services)
    {
        services.AddDefaultIdentity<Uceme.Model.Models.Security.ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<Uceme.Model.Models.Security.ApplicationUser, ApplicationDbContext>();

        services.AddAuthentication()
            .AddIdentityServerJwt();
    }

    private void SetupCors(IServiceCollection services)
    {
        CorsSettings? corsSettings = this.configuration.GetSection("CorsSettings").Get<CorsSettings>();
        if (corsSettings == null)
        {
            throw new InvalidDataException("missing corsSettings settings");
        }

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
                builder.WithOrigins(corsSettings.StrictPolicyHost ?? string.Empty)
                        .WithMethods("PUT", "DELETE", "GET", "POST");
            });
        });
    }
}
