namespace Uceme.API.Integration.Tests;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

internal static class JwtTestTokenBuilder
{
    internal static string CreateAccessToken(string issuer, string audience, TimeSpan? lifetime = null)
    {
        string pemPath = Path.Combine(AppContext.BaseDirectory, "TestData", "integration-test-rsa.pem");
        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(pemPath));
        var securityKey = new RsaSecurityKey(rsa.ExportParameters(true))
        {
            KeyId = TestIdpWireMock.KeyId,
        };
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: null,
            notBefore: now,
            expires: now.Add(lifetime ?? TimeSpan.FromHours(1)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
