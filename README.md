# DoctorLoan Web API

DoctorLoan Web API powers the broader DoctorLoan ecosystem: EMR management, booking flows, rehab products/services and back-office processes. The project follows a **modular monolithic** architecture on .NET 7, with clean separation between Domain/Application/Infrastructure and a growing list of business-oriented modules.

## Architecture & Main Components
- **Core (`src/Core`)**  
  - `DoctorLoan.Domain`: entities, enums, value objects, domain interfaces.  
  - `DoctorLoan.Application`: CQRS-based use cases (MediatR), DTO/mapping profiles, validation, service contracts.  
  - `DoctorLoan.Infrastructure`: EF Core + PostgreSQL persistence, caching, email, JWT, media storage and pipeline behaviours.
- **Modules (`src/Modules`)**: each bounded context (Authenticates, Bookings, Customers, Documents, MedicalRecord, News, Orders, Products, Users, …) ships its own `*.Application`, `*.Infrastructure`, `*.WebUI`. They are auto-registered via `AppDomainTypeFinder` and `IConfigService`, so new modules can be added without changing the core.
- **WebUI (`src/WebUI/DoctorLoan.WebAPI`)**: hosts the HTTP entry point, sets up middleware (JWT, localization, health checks, exception filter, NSwag) and exposes controllers implemented inside every module.
- **Deployment**: multi-stage Dockerfile plus `aws-beanstalk-tools-defaults.json` for AWS Elastic Beanstalk pipelines.

```
doctorloan-api/
├── src
│   ├── Core
│   │   ├── DoctorLoan.Application
│   │   ├── DoctorLoan.Domain
│   │   └── DoctorLoan.Infrastructure
│   ├── Modules
│   │   ├── Authenticates/
│   │   ├── Bookings/
│   │   ├── Commons/
│   │   ├── Customers/
│   │   ├── Documents/
│   │   ├── MedicalRecord/
│   │   ├── News/
│   │   ├── Orders/
│   │   ├── Products/
│   │   └── Users/
│   └── WebUI/DoctorLoan.WebAPI
└── DoctorLoan.sln
```

## Key Technologies
- .NET 7 (SDK pinned via `global.json` 8.0.302) with ASP.NET Core Web API.
- Entity Framework Core 7 + PostgreSQL, optional InMemory provider for tests.
- MediatR, FluentValidation, AutoMapper, NSwag (Swagger UI), JWT Bearer authentication.
- Serilog (console + Datadog hook outside Development), Health Checks, localization (`vi-VN` / `en-US`).
- MailKit/NETCore.MailKit for SMTP, in-memory cache for settings and app cache.

## Getting Started
1. **Prerequisites**
   - .NET SDK 8.0.302 (or a version compatible with net7.0).
   - PostgreSQL 14 or newer.
   - Node/Vite if you plan to run a front-end against the API.
2. **Clone & restore**
   ```bash
   git clone <repo-url>
   cd DOCTORLOAN-WebAPI/doctorloan-api
   dotnet restore
   ```
3. **Configuration**  
   Copy `src/WebUI/DoctorLoan.WebAPI/appsettings.json` to the environment-specific variant (Development/Staging/Production) and adjust:
   - `ConnectionStrings.DefaultConnection`
   - `JWTTokenConfiguration` (Key, Issuer, Audience, expiration)
   - `SystemConfiguration` (encryption key, default password, CORS)
   - `EmailConfiguration` (real SMTP, avoid committing secrets; use Secret Manager or env vars instead)
   - `MediaStorage.Type` (`Physical`, or extend with cloud implementations).
4. **Database prep**
   ```bash
   dotnet ef database update \
     --project src/WebUI/DoctorLoan.WebAPI/DoctorLoan.WebAPI.csproj \
     --startup-project src/WebUI/DoctorLoan.WebAPI
   ```
   `ApplicationDbContextInitialiser` seeds reference data: symptom groups, departments, roles, demo products and the initial admin account.
5. **Run the API**
   ```bash
   dotnet run --project src/WebUI/DoctorLoan.WebAPI
   ```
   - Swagger UI: `https://localhost:5001/swagger`
   - Health check: `https://localhost:5001/health`

### Docker workflow
```bash
cd doctorloan-api/src/WebUI/DoctorLoan.WebAPI
docker build -t doctorloan-api .
docker run -p 8080:80 --env-file .env doctorloan-api
```
Provide an `.env` file to inject connection strings, JWT keys, SMTP credentials, etc., instead of hard-coding them inside the image.

## Configuration Highlights
- `UseInMemoryDatabase`: set to `true` for quick integration tests without PostgreSQL.
- `SystemConfiguration`: controls `UserCodeLength`, `DefaultPrefixCode`, `DefaultPassword`, `AllowCORSUrl`, encryption keys, timeouts.
- `JWTTokenConfiguration`: HMAC SHA256 symmetric key; issuer/audience validation is enforced outside Development.
- `SerilogConfiguration`: log level overrides and Datadog HTTP sink (active when `ASPNETCORE_ENVIRONMENT != Development`).
- `EmailConfiguration`: MailKit SMTP (default Gmail SSL 465). Prefer app passwords and never commit plaintext secrets.
- `MediaStorage`: currently `Physical` (stored under `wwwroot`); extend by implementing `IMediaService` for S3/Blob/etc.
- `RateLimiting`: toggle the per-IP fixed-window limiter and tune `PermitLimit`, `WindowSeconds`, `QueueLimit` (defaults 100 req/min with no queue).

## Typical Runtime Flow
1. `Program.cs` calls `AddWebApplicationServices`, registering cross-cutting services, modules and middleware.
2. `ApplicationDbContextInitialiser` migrates and seeds the database automatically in Development.
3. MediatR pipeline behaviours (`UnhandledExceptionBehaviour`, `AuthorizationBehaviour`, `ValidationBehaviour`, `PerformanceBehaviour`) enforce logging, auth and validation consistently.
4. A global fixed-window rate limiter throttles requests per client IP before controller logic runs.
5. Each module’s controllers return DTOs projected via their AutoMapper profiles.
6. Localization middleware inspects the `Accept-Language` header (preferred `vi-VN`, fallback `en-US`).

## Notable Modules
- **Authenticates**: login, JWT issuance, refresh-token management.
- **Users**: internal users, roles (admin/leader/user), BCrypt SHA512 password storage.
- **Customers**: CRM-style customer records and lead lifecycle.
- **Bookings**: therapy appointment scheduling and ticket status.
- **MedicalRecord**: symptom catalog, treatment plans, rehab journey tracking.
- **Products / Orders**: catalog of services/products, order management, Payoo payment integration.
- **Documents / News / Commons**: CMS assets, downloadable documents, shared system settings.

## API Documentation
- Swagger/NSwag is exposed at `/swagger`.  
- During Debug builds, the MSBuild `NSwag` target runs automatically (see `DoctorLoan.WebAPI.csproj`).  
- Manual client generation:
  ```bash
  nswag run nswag.json /variables:Configuration=Debug
  ```

## Logging & Observability
- Development: Serilog console output (rendered compact JSON).  
- Staging/Production: forward logs to Datadog using values from `SerilogConfiguration` and your API key.  
- Health checks rely on `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore` and can be plugged into LBs/monitors.

## Security Notes
- Use `dotnet user-secrets` or environment variables for sensitive values (`ConnectionStrings`, `JWTTokenConfiguration.Key`, `EmailConfiguration.Password`).  
- Seeder creates `doctorloanadmin` with a default password—change it immediately after deployment.
- Update `SystemConfiguration.AllowCORSUrl` to whitelist the real front-end domains.
- Every endpoint now requires an authenticated JWT by default; add `[AllowAnonymous]` only on public controllers (e.g. login, content delivery).

## Testing & Quality
- No dedicated test projects yet. Recommended additions:
  - Unit tests for MediatR handlers inside the Application layer.
  - Integration tests with EF Core InMemory provider.
  - Contract tests generated from the OpenAPI spec (`nswag`).

## Contributing
1. Fork and create a feature branch.  
2. Keep the module structure intact; register new services via `IConfigService`.  
3. Add migrations with `dotnet ef migrations add <Name> --project src/Core/DoctorLoan.Infrastructure`.  
4. Open a PR with context, DB/API impact and verification steps.

---
Need deeper guidance (CI/CD pipeline, front-end integration, etc.)? Open an issue and we’ll expand the docs. Happy building!
