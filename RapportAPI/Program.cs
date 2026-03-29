using Microsoft.EntityFrameworkCore;
using RapportAPI.Data;
using RapportAPI.Models;
using RapportAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=rapport.db";
if (connectionString.Contains("rapport.db") && !Path.IsPathRooted(connectionString.Replace("Data Source=", "")))
{
    var dbPath = Path.Combine(builder.Environment.ContentRootPath, "rapport.db");
    connectionString = $"Data Source={dbPath}";
}

builder.Services.AddDbContext<DatabasContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddHttpClient<RapportService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DatabasContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    db.Database.Migrate();

    if (!db.Formaner.Any())
    {
        var gymkort = new Forman { Namn = "Gymkort", Kostnad = 3000, Aktiv = true };
        var friskvard = new Forman { Namn = "Friskvård", Kostnad = 5000, Aktiv = true };
        var massage = new Forman { Namn = "Massage", Kostnad = 2000, Aktiv = false };

        db.Formaner.AddRange(gymkort, friskvard, massage);
        db.SaveChanges();

        db.Ansokningar.AddRange(
            new Ansokan { FormanId = gymkort.Id, MedarbetarNamn = "Anna", Beviljad = true },
            new Ansokan { FormanId = gymkort.Id, MedarbetarNamn = "Erik", Beviljad = true },
            new Ansokan { FormanId = gymkort.Id, MedarbetarNamn = "Sara", Beviljad = true },
            new Ansokan { FormanId = friskvard.Id, MedarbetarNamn = "Johan", Beviljad = true },
            new Ansokan { FormanId = friskvard.Id, MedarbetarNamn = "Emma", Beviljad = true },
            new Ansokan { FormanId = massage.Id, MedarbetarNamn = "Lina", Beviljad = true }
        );
        db.SaveChanges();
    }

    if (!db.RapportPoster.Any())
    {
        db.RapportPoster.AddRange(
            new RapportPost
            {
                Titel = "Månadsrapport januari",
                Beskrivning = "Sammanställning av förmånsanvändning för januari.",
                SkapadAv = "HR"
            },
            new RapportPost
            {
                Titel = "Ekonomirapport Q1",
                Beskrivning = "Kostnader kopplade till aktiva förmåner under kvartal 1.",
                SkapadAv = "Ekonomi"
            }
        );
        db.SaveChanges();
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Ett fel inträffade vid databasinitiering. Appen fortsätter utan seed-data.");
}

app.Run();