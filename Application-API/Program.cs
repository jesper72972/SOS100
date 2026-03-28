using Microsoft.EntityFrameworkCore;
using Application_API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=applications.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("ReactAppPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();