# doctorloan-api
## Technologies
- ASP .Net Core 7
- Entity Framework Core 7
## Install Packages
- Npgsql.EntityFrameworkCore.PostgreSQL
- FluentValidation.AspNetCore
- FluentValidation.DependencyInjectionExtensions
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.AspNetCore.Authentication.OpenIdConnect
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
- Microsoft.AspNetCore.Identity.UI
- Microsoft.AspNetCore.Mvc.Formatters.Json
- Microsoft.AspNetCore.Mvc.NewtonsoftJson
- Microsoft.AspNetCore.SpaProxy
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.EntityFrameworkCore.Relational
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Binder
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
- Microsoft.Extensions.Options.ConfigurationExtensions
- Microsoft.Net.Http.Headers

## Config and Run
- Install PostgreSQL
- Create database doctorloan
- Run project
``` 
dotnet run
``` 
- Open browser and go to https://localhost:5001/swagger/index.html
- Login with user: admin, password: 123456
- Enjoy it!
```

``` 
dotnet ef migrations add InitialCreate
dotnet ef database update
```
