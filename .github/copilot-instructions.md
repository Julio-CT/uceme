<!-- Auto-generated guidance for AI coding agents working on the UCEME repo. -->
# Copilot / AI Agent Instructions — UCEME

Purpose: give focused, actionable context so an AI coding agent can be productive immediately.

- Scope: this repo is a .NET solution named `UCEME.sln` with multiple projects under `src/` (notably `Uceme.API`, `Uceme.UI`, `Uceme.Foundation`, `Uceme.Library`, `Uceme.Model`, and several `*.Tests`).

Quick Big Picture
- Layered backend + SPA frontend: `Uceme.API` is the REST API (Controllers under `src/Uceme.API/Controllers`). `Uceme.UI` is a React/ClientApp SPA served alongside the UI project. Shared domain models and business logic live in `Uceme.Model`, `Uceme.Library`, and `Uceme.Foundation`.
- Data: SQL initialization and DB scripts are in `dbscripts/` (look at `dbscripts/mssql_scripts` and `dbscripts/sqlucemedata`). The system generally expects a SQL Server backend (see docker and minikube helpers).
- Orchestration: Docker Compose variants live in `src/` and `dbscripts/`; there is a Kubernetes manifest `ucemek8s.yaml` for cluster deployments.

Important Files / Places to Inspect
- Solution file: `src/UCEME.sln` — entrypoint for builds.
- API controllers: `src/Uceme.API/Controllers/*` (example: `AppointmentController.cs`).
- UI client: `src/Uceme.UI/ClientApp/README.md` and `src/Uceme.UI/` for SPA integration and startup.
- DB scripts and init: `dbscripts/` and `dbscripts/mssql_scripts/run-initialization.sh`.
- Docs: `docs/Migrations.md` for DB migration conventions and `README.md` for repo orientation.
- Enforcement and conventions: `stylecop.json` and `Directory.Build.props` control formatting and analyzers.

Build / Run / Test Workflows (explicit commands)
- Build entire solution: `dotnet build src/UCEME.sln`
- Build API only: `dotnet build src/Uceme.API/Uceme.API.csproj`
- Run API in watch mode (dev): `dotnet watch run --project src/Uceme.API/Uceme.API.csproj`
- Run UI in watch mode (dev): `dotnet watch run --project src/Uceme.UI/Uceme.UI.csproj` (ClientApp may have its own npm/yarn steps inside `ClientApp`).
- Publish artifacts: `dotnet publish <project>.csproj` (see workspace tasks `publish` and `publishAPI`).
- Docker / local DB: use `dbscripts/docker-compose.yml` or `src/docker-compose*.yml` for local dev database containers.
- Tests: run test projects under `src/*Tests` via `dotnet test` or use the repo's test task(s).

Project-specific Patterns & Conventions
- Layering: prefer putting shared business logic in `Uceme.Foundation`/`Uceme.Library`, keeping controllers thin (controller → service → repository). When changing behavior, update tests in the corresponding `*.Tests` project.
- Naming: follow .NET PascalCase for types and methods. The repo uses StyleCop — keep code formatted to satisfy analyzers.
- Configuration: look for central settings in `Directory.Build.props` and per-project `appsettings*.json` files.
- Database migrations: Migrations are handled outside of in-repo EF flows in some cases — check `docs/Migrations.md` and `dbscripts` before assuming EF migrations are authoritative.

Integration Points & External Dependencies
- SQL Server: DB scripts assume Microsoft SQL Server; containers and scripts reference SQL Server artifacts (`mssql_scripts`).
- Docker & Kubernetes: `docker-compose.yml`, `dbscripts/docker-compose.yml`, and `ucemek8s.yaml` are used for containerized dev and k8s deployments.
- External services / secrets: check `dbscripts/sqlucemedata/secrets/` and environment variables used by API and UI startup.

Guidance for Making Changes (AI agent rules)
- Make minimal, focused edits. Prefer small diffs and preserve public APIs unless the change is explicitly requested.
- Always update or add unit tests in the related `*Tests` project for behavioral changes. Run `dotnet test` on the modified test project(s).
- Use `apply_patch` for file edits. Keep indentation and code style consistent with existing files (use repository's existing patterns — StyleCop is present).
- For API changes: update controller, service, and model layers in that order; add/adjust DTOs and mapping in `Uceme.Model` or `Uceme.Library` as appropriate.
- When touching DB logic, reference `docs/Migrations.md` and `dbscripts/` — do not assume automatic migrations in production.

Examples (where to look for patterns)
- Controller example: `src/Uceme.API/Controllers/AppointmentController.cs` (request/response patterns, authorization attributes).
- Tests: inspect `Uceme.Foundation.Tests` and `Uceme.Library.Tests` for how services and repositories are tested.

Common Gotchas
- Many dev workflows rely on local containerized SQL; missing the database container will cause integration tests or API startup to fail.
- There are multiple compose files and scripts — double-check which compose file a local developer uses (some are under `dbscripts/` and some under `src/`).
- Respect `Directory.Build.props` and `stylecop.json` — CI enforces analyzers.

If you change the repository structure
- Update the solution `src/UCEME.sln` and ensure new projects are added to CI/build scripts.

When you finish a change
- Run the relevant `dotnet build` and `dotnet test` for affected projects. If you changed public API surface, run the UI to ensure no runtime errors.

Contact / Owner Notes
- Repo owner: `Julio-CT` (commit history shows frequent activity). For unclear domain semantics, consult `docs/General.md`.

Ask the human: If any architectural decision or DB migration convention seems ambiguous, ask for the expected runtime environment (local docker vs specific cloud DB) and whether a migration should be applied automatically or by ops.

-- End of instructions
