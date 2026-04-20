---
name: WireMock snapshot integration tests
overview: "Turn [Uceme.API.Integration.Tests](src/Uceme.API.Integration.Tests) into real ASP.NET Core integration tests: `WebApplicationFactory` + EF InMemory, WireMock as the OIDC authority for JWT-protected routes, Shouldly for structural assertions, and Verify for committed JSON response snapshots. This addresses the repo rule about HTTP boundaries while acknowledging the API currently has no outbound `HttpClient` except JWT metadata retrieval."
todos:
  - id: factory-infra
    content: Add API project + Mvc.Testing + EF InMemory + Verify packages; implement ApiWebApplicationFactory with Testing env, InMemory DbContext, TokenSettings override
    status: completed
  - id: wiremock-oidc
    content: Implement WireMock OIDC discovery + JWKS + JwtTestTokenBuilder; integration test proving protected route hits WireMock and returns 200
    status: completed
  - id: startup-testing-hooks
    content: Gate Database.Migrate (and HTTPS redirect if needed) when environment is Testing
    status: completed
  - id: snapshot-suite
    content: Add Verify snapshot tests for AllowAnonymous endpoints (Blog, Settings, then other controllers with seed helpers); Shouldly on status codes
    status: completed
  - id: auth-snapshots
    content: Extend snapshots to addpost/deletepost/onpostuploadasync with Bearer token + temp BlogImagesDir
    status: completed
  - id: quality-gates
    content: Run dotnet build, dotnet test, dotnet format; fix analyzer issues
    status: completed
isProject: false
---

# Massive WireMock + Shouldly + snapshot integration coverage

## Current state

- [WireMockHttpBoundaryTests.cs](src/Uceme.API.Integration.Tests/WireMockHttpBoundaryTests.cs) only proves WireMock returns a stubbed body; it does **not** exercise [Uceme.API](src/Uceme.API).
- [Uceme.API.Integration.Tests.csproj](src/Uceme.API.Integration.Tests/Uceme.API.Integration.Tests.csproj) has no reference to the API, no `Microsoft.AspNetCore.Mvc.Testing`, and no snapshot library.
- Grep shows **no** production `HttpClient` / `IHttpClientFactory` in `src` except tests. The meaningful HTTP boundary for integration tests is **JWT Bearer**: [Startup.cs](src/Uceme.API/Startup.cs) sets `options.Authority` from `TokenSettings`, so the middleware will fetch OpenID metadata from that URL when validating tokens on protected endpoints.

```mermaid
sequenceDiagram
  participant Test as IntegrationTest
  participant API as Uceme_API_TestServer
  participant WM as WireMock_OIDC
  Test->>API GET protected with Bearer JWT
  API->>WM GET well-known openid-configuration
  WM-->>API discovery JSON
  API->>WM GET jwks_uri
  WM-->>API JWKS
  API-->>Test 200 JSON body
```

## Architecture decisions

| Concern | Choice |
|--------|--------|
| Host under test | `WebApplicationFactory<Startup>` (same pattern as existing [Startup](src/Uceme.API/Startup.cs) / [Program.cs](src/Uceme.API/Program.cs) WebHost setup) |
| Database | EF Core **InMemory** in tests, mirroring [BlogServiceTests](src/Uceme.Library.Tests/Services/BlogServiceTests.cs) use of `UseInMemoryDatabase` + [OperationalStoreOptionsMigrations](src/Uceme.Model/Data/OperationalStoreOptionsMigrations.cs) |
| WireMock role | Stub **OIDC discovery + JWKS** at a dynamic base URL; override `TokenSettings:Authority` (and `Audience`, `RequireHttpsMetadata`) via `IConfiguration` / `ConfigureTestServices` / `WebHostBuilder` so the API calls WireMock instead of a real IdP |
| Snapshots | **[Verify](https://github.com/VerifyTests/Verify)** (`Verify` + `Verify.MSTest`) — generates `*.verified.json` (or `.txt`) next to tests; industry standard for .NET snapshot workflows |
| Shouldly | Use for status codes, headers, and simple equality; use Verify for full response bodies (per workspace rule: both should appear in the suite) |

## Required production-code adjustments (minimal)

1. **Skip `Database.Migrate()` in integration runs** — `Migrate()` is not viable with the InMemory provider (and may throw). Use `IWebHostEnvironment` / `IHostEnvironment`: if environment name is `Testing` (set by the factory), skip migration (or replace with `EnsureCreated()` only when using InMemory — prefer skip to avoid schema drift vs migrations).
2. **Optional: HTTPS redirection** — If `CreateClient()` sees 307 loops from [UseHttpsRedirection](src/Uceme.API/Startup.cs), gate `UseHttpsRedirection()` behind `!env.IsEnvironment("Testing")` (same as above).

## Test infrastructure (shared fixtures)

Add to the integration project:

- **`ApiWebApplicationFactory`** — `ProjectReference` to [Uceme.API.csproj](src/Uceme.API/Uceme.API.csproj); package refs aligned to **net7.0**: `Microsoft.AspNetCore.Mvc.Testing`, `Microsoft.EntityFrameworkCore.InMemory`, `Verify`, `Verify.MSTest`, plus existing `WireMock.Net` / `Shouldly`.
- **`WireMockIdpFixture`** (or static helper) — starts `WireMockServer`, registers mappings for:
  - `/.well-known/openid-configuration` with `issuer`, `jwks_uri`, `authorization_endpoint` (can be dummy), etc.
  - JWKS document matching a **fixed RSA key pair** committed in test-only code (or generated once and stored as base64 in a test resource).
- **`JwtTestTokenBuilder`** — uses `Microsoft.IdentityModel.Tokens` / `System.IdentityModel.Tokens.Jwt` to mint JWTs with `aud` = configured audience, `iss` = WireMock issuer, `kid` matching JWKS, signed with the private key.
- **Seed helper** — after factory creates scope, resolve `ApplicationDbContext`, add entities with **fixed dates/ids** so snapshots stay stable (reuse seed shapes from [BlogServiceTests](src/Uceme.Library.Tests/Services/BlogServiceTests.cs) where possible).

Factory configuration sketch:

- `factory.WithWebHostBuilder(builder => { builder.UseEnvironment("Testing"); builder.ConfigureAppConfiguration((ctx, cfg) => cfg.AddInMemoryCollection(...)); })` to point `TokenSettings` at `wireMockServer.Url` and set `ConnectionStrings:UcemeConnection` to a sentinel (overridden by InMemory anyway).
- `ConfigureTestServices`: remove/replace `DbContextOptions<ApplicationDbContext>` registration to `UseInMemoryDatabase` with a unique database name per factory instance.

## Coverage phases (what “massive” means here)

**Phase A — Anonymous JSON contracts (snapshots + Shouldly)**  
Hit the API via `HttpClient` from the factory; assert `StatusCode` with Shouldly; `await VerifyJson(await response.Content.ReadAsStringAsync())` for bodies.

Prioritize endpoints already marked `[AllowAnonymous]`:

- Blog: `getblogsubset`, `getbloglist`, `getallposts`, `getpost` — include **error** snapshots where stable (e.g. empty DB 404/500 behavior as implemented today).
- [SettingsController](src/Uceme.API/Controllers/SettingsController.cs): `getsettings` — no DB; fastest win.
- Then expand to [HospitalController](src/Uceme.API/Controllers/HospitalController.cs), [TechniqueController](src/Uceme.API/Controllers/TechniqueController.cs), [AppointmentController](src/Uceme.API/Controllers/AppointmentController.cs) anonymous routes, [ScheduleController](src/Uceme.API/Controllers/ScheduleController.cs), [ContactController](src/Uceme.API/Controllers/ContactController.cs), [HomeController](src/Uceme.API/Controllers/HomeController.cs) — each needs **minimal seed data** where the service reads EF; add one focused seeder per area to avoid huge setup.

**Phase B — JWT + WireMock (protected blog actions)**  
For [BlogController](src/Uceme.API/Controllers/BlogController.cs) routes without `[AllowAnonymous]` (`deletepost`, `addpost`, `onpostuploadasync`):

- Send `Authorization: Bearer {token}` from `JwtTestTokenBuilder`.
- Assert WireMock received at least one call to discovery/JWKS (optional `Shouldly` on `server.LogEntries` or search API) to prove the stub was used.
- Snapshots: JSON bodies for success paths; for `onpostuploadasync`, use `MultipartFormDataContent` and snapshot **stable** outcomes (e.g. 400 validation) first; file-system success requires a writable temp `BlogImagesDir` injected via test configuration pointing at `Path.GetTempPath()` / `ITempData` directory created in test setup.

**Phase C — Keep or retire the ping test**  
Either delete [WireMockHttpBoundaryTests](src/Uceme.API.Integration.Tests/WireMockHttpBoundaryTests.cs) once OIDC tests prove WireMock wiring, or keep a single “sanity” test — avoid duplicate noise.

## Verify hygiene

- Register **scrubbers** for any remaining non-determinism (guids in upload responses, timestamps if any leak into JSON).
- Commit `*.verified.*` files; document in PR that `dotnet test` updates received files when APIs intentionally change (use Verify’s accept workflow).

## Quality gates (per [.cursor/rules/plan-delivery-quality-gates.mdc](.cursor/rules/plan-delivery-quality-gates.mdc))

After implementation: `dotnet build` on [UCEME.sln](src/UCEME.sln), `dotnet test`, `dotnet format`, and ensure CI runs the new tests.

## Risk notes

- **Dual JWT schemes** (`default` and `alt` in [Startup.cs](src/Uceme.API/Startup.cs)): if `AuthorityAlt` is null in test config, the `alt` handler may misconfigure — supply explicit test values or register only what tests need via configuration.
- **Identity / ApiAuthorizationDbContext**: InMemory + `OperationalStoreOptionsMigrations` is already proven in library tests; API DI must receive the same options shape the context constructor expects.
