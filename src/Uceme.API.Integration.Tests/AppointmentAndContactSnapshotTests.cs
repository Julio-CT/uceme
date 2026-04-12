namespace Uceme.API.Integration.Tests;

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
public sealed class AppointmentAndContactSnapshotTests : VerifyBase
{
    [TestMethod]
    public async Task Appointment_get_days_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedAppointment(factory);

        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/appointment/days/7101", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Appointment_get_hours_matches_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        this.SeedAppointment(factory);

        using HttpClient client = factory.CreateClient();
        var url = "/api/appointment/hours?weekDay=3&hospitalId=7101&day=15&month=6&year=2020";
        using HttpResponseMessage response = await client.GetAsync(new Uri(url, UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Appointment_get_hours_missing_hospital_bad_request_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        using HttpClient client = factory.CreateClient();
        using HttpResponseMessage response = await client.GetAsync(new Uri("/api/appointment/hours?weekDay=1&hospitalId=&day=1&month=1&year=2020", UriKind.Relative)).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        string stable = JsonSnapshotHelpers.RemoveRootTraceId(body);
        await VerifyJson(stable).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Contact_contactemail_missing_email_bad_request_snapshot()
    {
        using var factory = new ApiWebApplicationFactory(IntegrationAssemblySetup.IdpAuthorityBaseUrl);
        using HttpClient client = factory.CreateClient();
        var json = JsonSerializer.Serialize(new { name = "x", message = "y" });
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await client.PostAsync(new Uri("/api/contact/contactemail", UriKind.Relative), content).ConfigureAwait(false);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        await VerifyJson(body).ConfigureAwait(false);
    }

    private void SeedAppointment(ApiWebApplicationFactory factory)
    {
        using IServiceScope scope = factory.Services.CreateScope();
        ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IntegrationTestDataSeeder.SeedTurnosForAppointment(db);
    }
}
