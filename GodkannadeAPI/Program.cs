using GodkannadeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddDbContext<ApprovalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5118",
                "http://localhost:5173",
                "http://localhost:5174",
                "https://app-sos100-formaner.azurewebsites.net",
                "https://app-sos100-application-chfqc9fxeubbf7aw.swedencentral-01.azurewebsites.net",
                "https://app-sos100-godkannade.azurewebsites.net",
                "https://app-sos100-rapport-b6bncnaga4h6e7du.swedencentral-01.azurewebsites.net",
                "https://app-sos100-status-formaner.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApprovalDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.UseCors("ReactAppPolicy");

app.MapGet("/", () => "GodkannandeAPI is running");

app.Run();