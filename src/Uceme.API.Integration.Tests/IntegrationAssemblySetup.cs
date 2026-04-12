namespace Uceme.API.Integration.Tests;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WireMock.Server;

/// <summary>
/// One WireMock-backed OIDC stub per test assembly (avoids parallel port binding issues).
/// </summary>
[TestClass]
public static class IntegrationAssemblySetup
{
    public static WireMockServer IdpServer { get; private set; } = null!;

    public static string IdpAuthorityBaseUrl { get; private set; } = string.Empty;

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext testContext)
    {
        ArgumentNullException.ThrowIfNull(testContext);
        IdpServer = WireMockServer.Start();
        IdpAuthorityBaseUrl = IdpServer.Url!.TrimEnd('/');
        TestIdpWireMock.ConfigureOidcStubs(IdpServer, IdpAuthorityBaseUrl);
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        IdpServer?.Stop();
        IdpServer?.Dispose();
    }
}
