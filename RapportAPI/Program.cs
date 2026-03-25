using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=rapport.db"));

var app = builder.Build();

// Stäng av HTTPS (viktigt nu)
// app.UseHttpsRedirection();

app.MapControllers();

app.Run();