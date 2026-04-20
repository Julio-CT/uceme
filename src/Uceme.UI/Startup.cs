using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Uceme.Foundation.Utilities;
using Uceme.Library.Services;
using Uceme.Model.Data;
using Uceme.Model.Models.Security;
using Uceme.Model.Settings;

namespace Uceme.UI;

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
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.OnAppendCookie = cookieContext =>
                CheckSameSite(cookieContext.CookieOptions, cookieContext.Context);
            options.OnDeleteCookie = cookieContext =>
                CheckSameSite(cookieContext.CookieOptions, cookieContext.Context);
        });

        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddAuthentication()
            .AddIdentityServerJwt();
    }

    private static void CheckSameSite(CookieOptions options, HttpContext context)
    {
        if (options.SameSite == SameSiteMode.None)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            if (DisallowsSameSiteNone(userAgent))
            {
                options.SameSite = SameSiteMode.Unspecified;
            }
        }
    }

    private static bool DisallowsSameSiteNone(string userAgent)
    {
        // Cover all iOS based browsers here. This includes:
        // - Safari on iOS 12 for iPhone, iPod Touch, iPad
        // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
        // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
        // All of which are broken by SameSite=None, because they use the iOS networking stack
        if (userAgent.Contains("CPU iPhone OS 12", System.StringComparison.InvariantCultureIgnoreCase)
         || userAgent.Contains("iPad; CPU OS 12", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
        // - Safari on Mac OS X
        // This does not include:
        // - Chrome on Mac OS X
        // Because they do not use the Mac OS networking stack.
        if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14", System.StringComparison.InvariantCultureIgnoreCase) &&
            userAgent.Contains("Version/", System.StringComparison.InvariantCultureIgnoreCase)
            && userAgent.Contains("Safari", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        // Cover Chrome 50-69, because some versions are broken by SameSite=None,
        // and none in this range require it.
        // Note: this covers some pre-Chromium Edge versions,
        // but pre-Chromium Edge does not require SameSite=None.
        if (userAgent.Contains("Chrome/5", System.StringComparison.InvariantCultureIgnoreCase)
         || userAgent.Contains("Chrome/6", System.StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return false;
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
