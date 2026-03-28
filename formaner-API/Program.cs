using formaner_API.Data;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using formaner_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<DbService>(options =>
    
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5118",
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
    var services = scope.ServiceProvider;
    var dbcontext = services.GetRequiredService<DbService>();
    dbcontext.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.MapScalarApiReference();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("ReactAppPolicy");

app.MapControllers();

app.Run();