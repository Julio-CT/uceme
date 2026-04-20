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
public sealed class HospitalTechniqueHomeScheduleSnapshotTests : VerifyBase
{
    [TestMethod]
    public async Task Hospital_get_list_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedCore(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/hospital", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Hospital_gethospital_not_found_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedCore(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/hospital/gethospital?hostpitalId=99999", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Technique_gettechniques_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedCore(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/technique/gettechniques", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Technique_gettechnique_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedCore(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/technique/gettechnique?techinqueId=5001", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Home_getmedicominvista_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedCore(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/home/getmedicominvista", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Home_mostrarfotos_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedCore(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/home/mostrarfotos", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Schedule_hospitals_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedCore(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/schedule/hospitals", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    private void SeedCore(ApiWebApplicationFactory factory)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IntegrationTestDataSeeder.SeedHospitalTechniqueHome(db);
    }
}
