namespace Uceme.API.Integration.Tests;

using System;
using System.Net;
using System.Net.Http;
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
public sealed class BlogAnonymousApiSnapshotTests : VerifyBase
{
    [TestMethod]
    public async Task Settings_getsettings_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/settings/getsettings", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_getblogsubset_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        SeedBlog(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/blog/getblogsubset?amount=2", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_getbloglist_page1_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        SeedBlog(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/blog/getbloglist?page=1", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_getallposts_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        SeedBlog(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/blog/getallposts", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_getpost_by_slug_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        SeedBlog(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/blog/getpost?slug=snap-newest", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Blog_getpost_empty_slug_bad_request_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/blog/getpost?slug=", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        string stable = JsonSnapshotHelpers.RemoveRootTraceId(body);
        await VerifyJson(stable).ConfigureAwait(false);
    }

    private void SeedBlog(ApiWebApplicationFactory factory)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IntegrationTestDataSeeder.SeedBlog(db);
    }
}
