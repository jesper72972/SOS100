using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using status_formaner_API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5174",
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
    var appDbContext = services.GetRequiredService<AppDbContext>();
    appDbContext.Database.Migrate();
}


{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("ReactAppPolicy");
app.MapControllers();

app.Run();
