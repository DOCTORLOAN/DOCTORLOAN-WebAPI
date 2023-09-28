using DoctorLoan.Infrastructure.Persistence;
using DoctorLoan.Infrastructure.Services;
using DoctorLoan.WebAPI.Frameworks.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var typeFinder = new AppDomainTypeFinder();
var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddWebApplicationServices(builder.Configuration, typeFinder, isDevelopment);

if (!isDevelopment)
{
    builder.Host.UseSerilog();
}

var app = builder.Build();

if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.MapControllers().AllowAnonymous();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
    app.MapControllers();

app.UseDeveloperExceptionPage();
app.UseMigrationsEndPoint();
app.MapControllers().AllowAnonymous();

// Initialise and seed database
using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
}

app.ConfigWebApplication(typeFinder);

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
