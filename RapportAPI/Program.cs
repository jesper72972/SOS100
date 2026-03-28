using Microsoft.EntityFrameworkCore;
using RapportAPI.Data;
using RapportAPI.Models;
using RapportAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabasContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<RapportService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabasContext>();
    db.Database.Migrate();

    if (!db.Formaner.Any())
    {
        var gymkort = new Forman
        {
            Namn = "Gymkort",
            Kostnad = 3000,
            Aktiv = true
        };

        var friskvard = new Forman
        {
            Namn = "Friskvård",
            Kostnad = 5000,
            Aktiv = true
        };

        var massage = new Forman
        {
            Namn = "Massage",
            Kostnad = 2000,
            Aktiv = false
        };

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

app.Run();