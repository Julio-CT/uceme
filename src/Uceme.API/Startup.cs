namespace Uceme.Api;

using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        IConfigurationSection appSettingsSection = this.Configuration.GetSection("AppSettings");

        services.Configure<AppSettings>(appSettingsSection);
        services.Configure<AuthMessageSenderSettings>(this.Configuration.GetSection("EmailSettings"));

        AppSettings appSettings = new AppSettings();
        appSettingsSection.Bind(appSettings);

        string? ucemeConnection = this.Configuration.GetConnectionString("UcemeConnection");

        if (ucemeConnection == null)
        {
            throw new InvalidDataException("missing UcemeConnection settings");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ucemeConnection).EnableSensitiveDataLogging());

        services.AddControllers();

        this.SetupCors(services);
        this.SetupAuthentication(services);

        this.SetupDependecyInjection(services);
        this.SetupSwagger(services);
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

            SwaggerSettings? swaggerSettings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerSettings?.SwaggerUri?.ToString(), swaggerSettings?.SwaggerApp);
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        CorsSettings? corsSettings = this.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
        if (corsSettings == null)
        {
            throw new InvalidDataException("missing cors settings");
        }

        if (corsSettings.UseStrictPolicy)
        {
            _ = app.UseCors(this.strictPolicy);
        }
        else
        {
            _ = app.UseCors(this.relaxedPolicy);
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private void SetupAuthentication(IServiceCollection services)
    {
        TokenSettings? tokenSettings = this.Configuration.GetSection("TokenSettings").Get<TokenSettings>();
        if (tokenSettings == null)
        {
            throw new InvalidDataException("missing token settings");
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("default", options =>
        {
            options.Audience = tokenSettings.Audience;
            options.Authority = tokenSettings.Authority;
            options.RequireHttpsMetadata = tokenSettings.RequireHttpsMetadata;
        }).AddJwtBearer("alt", options =>
        {
            options.Audience = tokenSettings.AudienceAlt;
            options.Authority = tokenSettings.AuthorityAlt;
            options.RequireHttpsMetadata = tokenSettings.RequireHttpsMetadataAlt;
        });

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("default", "alt")
                .Build();
        });

        services.AddMvc(config =>
        {
            config.Filters.Add(new AuthorizeFilter());
        });
    }

    private void SetupCors(IServiceCollection services)
    {
        CorsSettings? corsSettings = this.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
        if (corsSettings == null)
        {
            throw new InvalidDataException("missing Swagger settings");
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

    private void SetupDependecyInjection(IServiceCollection services)
    {
        services.AddSingleton<IConfiguration>(this.Configuration);

        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        services.AddTransient<ISmtpClient, SmtpClientWrapper>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IMedicoService, MedicoService>();
        services.AddTransient<IFotosService, FotosService>();
        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IHospitalService, HospitalService>();
        services.AddTransient<IAppointmentService, AppointmentService>();
        services.AddTransient<ITechniqueService, TechniqueService>();
        services.AddTransient<IScheduleService, ScheduleService>();
    }

    private void SetupSwagger(IServiceCollection services)
    {
        SwaggerSettings? swaggerSettings = this.Configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
        if (swaggerSettings == null)
        {
            throw new InvalidDataException("missing Swagger settings");
        }

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerSettings.SwaggerVersion, new OpenApiInfo { Title = swaggerSettings.SwaggerApp, Version = swaggerSettings.SwaggerVersion });
#pragma warning disable CA1308 // Normalize strings to uppercase
            options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter the bearer token",
            });
#pragma warning restore CA1308 // Normalize strings to uppercase
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
             {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearerAuth",
                    },
                },
                System.Array.Empty<string>()
             },
            });
        });
    }
}
