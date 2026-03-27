using RapportAPI.Services;
using RapportAPI.Data;
using RapportAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<RapportService>();

builder.Services.AddDbContext<DatabasContext>(options =>
    options.UseSqlite("Data Source=/home/site/wwwroot/rapport.db"));

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabasContext>();

    db.Database.EnsureCreated();  //skapar ifall inte finns, tagit bort EnsureDeleted från testfas

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
}

app.Run();