namespace Uceme.API.Integration.Tests;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Uceme.Model.Data;
using VerifyMSTest;
using VerifyTests;

[TestClass]
[DoNotParallelize]
public sealed class BlogAuthenticatedApiIntegrationTests : VerifyBase
{
    [TestMethod]
    public async Task JwtBearer_uses_wiremock_oidc_metadata()
    {
        IntegrationAssemblySetup.IdpServer.ResetLogEntries();

        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedForDelete(factory);

        string token = JwtTestTokenBuilder.CreateAccessToken(
            IntegrationAssemblySetup.IdpAuthorityBaseUrl,
            "Uceme.UIAPI");

        using HttpClient client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using HttpResponseMessage response = await client.DeleteAsync(new Uri("/api/blog/deletepost/2001", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        bool sawDiscovery = IntegrationAssemblySetup.IdpServer.LogEntries.Any(e =>
            e.RequestMessage.Path.Contains("/.well-known/openid-configuration", StringComparison.Ordinal));
        bool sawJwks = IntegrationAssemblySetup.IdpServer.LogEntries.Any(e =>
            e.RequestMessage.Path.Contains("/.well-known/jwks", StringComparison.Ordinal));

        sawDiscovery.ShouldBeTrue();
        sawJwks.ShouldBeTrue();
    }

    [TestMethod]
    public async Task Blog_addpost_authenticated_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        string token = JwtTestTokenBuilder.CreateAccessToken(
            IntegrationAssemblySetup.IdpAuthorityBaseUrl,
            "Uceme.UIAPI");

        using HttpClient client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var payload = new
        {
            idBlog = 0,
            titulo = "Auth new post",
            fecha = "2022-06-15",
            foto = "pic.webp",
            texto = "Hello auth",
            slug = "auth-slug-unique-1",
            seoTitle = "seo auth",
            metaDescription = "meta auth",
        };
        using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await client.PostAsync(new Uri("/api/blog/addpost", UriKind.Relative), content).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_deletepost_authenticated_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedForDelete(factory);

        string token = JwtTestTokenBuilder.CreateAccessToken(
            IntegrationAssemblySetup.IdpAuthorityBaseUrl,
            "Uceme.UIAPI");

        using HttpClient client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using HttpResponseMessage response = await client.DeleteAsync(new Uri("/api/blog/deletepost/2001", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_onpostuploadasync_rejects_non_image_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.EnsureBlogImageDir(factory);

        string token = JwtTestTokenBuilder.CreateAccessToken(
            IntegrationAssemblySetup.IdpAuthorityBaseUrl,
            "Uceme.UIAPI");

        using HttpClient client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var form = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        form.Add(fileContent, "file", "bad.txt");

        using HttpResponseMessage response = await client.PostAsync(new Uri("/api/blog/onpostuploadasync", UriKind.Relative), form).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_onpostuploadasync_png_success_scrubbed_filename_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.EnsureBlogImageDir(factory);

        string token = JwtTestTokenBuilder.CreateAccessToken(
            IntegrationAssemblySetup.IdpAuthorityBaseUrl,
            "Uceme.UIAPI");

        using HttpClient client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        byte[] png = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");

        using var form = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(png);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        form.Add(fileContent, "file", "x.png");

        using HttpResponseMessage response = await client.PostAsync(new Uri("/api/blog/onpostuploadasync", UriKind.Relative), form).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        string scrubbed = Regex.Replace(body, @"Blog[a-f0-9]{32}", "Blog{scrubbedGuid}", RegexOptions.IgnoreCase);
        await Verify(scrubbed).ConfigureAwait(false);
    }

    private void SeedForDelete(ApiWebApplicationFactory factory)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IntegrationTestDataSeeder.SeedBlogForMutation(db);
    }

    private void EnsureBlogImageDir(ApiWebApplicationFactory factory)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
        string? dir = configuration?["AppSettings:BlogImagesDir"];
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}
