namespace Uceme.API.Integration.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Uceme.API;
using Uceme.Model.Data;

/// <summary>
/// Boots the real API with an in-memory database and WireMock-backed OIDC authority URL from configuration.
/// </summary>
public sealed class ApiWebApplicationFactory : WebApplicationFactory<Startup>
{
    private readonly string idpAuthorityBaseUrl;
    private readonly string inMemoryDatabaseName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiWebApplicationFactory"/> class.
    /// </summary>
    /// <param name="idpAuthorityBaseUrl">OpenID issuer base URL (no trailing slash), e.g. WireMock root URL.</param>
    /// <param name="inMemoryDatabaseName">Optional fixed in-memory database name for isolated fixtures.</param>
    public ApiWebApplicationFactory(string idpAuthorityBaseUrl, string? inMemoryDatabaseName = null)
    {
        ArgumentNullException.ThrowIfNull(idpAuthorityBaseUrl);
        this.idpAuthorityBaseUrl = idpAuthorityBaseUrl.TrimEnd('/');
        this.inMemoryDatabaseName = inMemoryDatabaseName ?? $"uceme_api_int_{Guid.NewGuid():N}";
    }

    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.UseEnvironment("Testing");

        // Avoid binding to fixed ports from appsettings or launchSettings during TestServer startup.
        builder.UseSetting("urls", "http://127.0.0.1:0");
        builder.UseUrls("http://127.0.0.1:0");

        builder.ConfigureAppConfiguration(
            (_, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["Urls"] = "http://127.0.0.1:0",
                        ["ConnectionStrings:UcemeConnection"] = "Server=(localdb)\\mssqllocaldb;Database=IntegrationTestUnused;Trusted_Connection=True;",
                        ["TokenSettings:Authority"] = this.idpAuthorityBaseUrl,
                        ["TokenSettings:Audience"] = "Uceme.UIAPI",
                        ["TokenSettings:RequireHttpsMetadata"] = "false",
                        ["TokenSettings:AuthorityAlt"] = this.idpAuthorityBaseUrl,
                        ["TokenSettings:AudienceAlt"] = "Uceme.UIAPI",
                        ["TokenSettings:RequireHttpsMetadataAlt"] = "false",
                        ["AppSettings:BlogImagesDir"] = Path.Combine(Path.GetTempPath(), "uceme_blog_int_tests"),
                    });
            });

        builder.ConfigureServices(
            services =>
            {
                foreach (var d in services.Where(x => x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)).ToList())
                {
                    services.Remove(d);
                }

                foreach (var d in services.Where(x => x.ServiceType == typeof(ApplicationDbContext)).ToList())
                {
                    services.Remove(d);
                }

                services.AddDbContext<ApplicationDbContext>(
                    options => options.UseInMemoryDatabase(this.inMemoryDatabaseName));

                if (!services.Any(d => d.ServiceType == typeof(IOptions<OperationalStoreOptions>)))
                {
                    services.AddSingleton<IOptions<OperationalStoreOptions>>(new OperationalStoreOptionsMigrations());
                }
            });
    }
}
