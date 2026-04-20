namespace Uceme.API.Integration.Tests;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

internal static class TestIdpWireMock
{
    internal const string KeyId = "integration-test-key";

    /// <summary>
    /// Registers OpenID discovery and JWKS for the JWT bearer handler (Authority = issuer base URL).
    /// </summary>
    /// <param name="server">WireMock server instance.</param>
    /// <param name="issuerBaseUrl">Issuer URL without trailing slash.</param>
    internal static void ConfigureOidcStubs(WireMockServer server, string issuerBaseUrl)
    {
        string issuer = issuerBaseUrl.TrimEnd('/');
        string jwksUri = $"{issuer}/.well-known/jwks";

        var discoveryDoc = new
        {
            issuer,
            jwks_uri = jwksUri,
            authorization_endpoint = $"{issuer}/authorize",
            token_endpoint = $"{issuer}/token",
            response_types_supported = new[] { "id_token", "token" },
            subject_types_supported = new[] { "public" },
            id_token_signing_alg_values_supported = new[] { "RS256" },
        };
        string discovery = JsonSerializer.Serialize(discoveryDoc);

        string jwks = BuildJwksJson();

        server
            .Given(Request.Create().WithPath("/.well-known/openid-configuration").UsingGet())
            .RespondWith(Response.Create().WithBody(discovery).WithHeader("Content-Type", "application/json"));

        server
            .Given(Request.Create().WithPath("/.well-known/jwks").UsingGet())
            .RespondWith(Response.Create().WithBody(jwks).WithHeader("Content-Type", "application/json"));
    }

    private static string BuildJwksJson()
    {
        string pemPath = Path.Combine(AppContext.BaseDirectory, "TestData", "integration-test-rsa.pem");
        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(pemPath));
        RSAParameters p = rsa.ExportParameters(false);
        string n = Base64UrlEncoder.Encode(p.Modulus!);
        string e = Base64UrlEncoder.Encode(p.Exponent!);
        var jwksObj = new
        {
            keys = new[]
            {
                new { kty = "RSA", use = "sig", kid = KeyId, alg = "RS256", n, e },
            },
        };
        return JsonSerializer.Serialize(jwksObj);
    }
}
