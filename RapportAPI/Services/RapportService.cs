using Microsoft.EntityFrameworkCore;
using RapportAPI.Data;

namespace RapportAPI.Services;

public class RapportService
{
    private readonly DatabasContext _databas;

    public RapportService(DatabasContext databas)
    {
        _databas = databas;
    }

    public object HamtaStatistik()
    {
        var kostnadPerForman = _databas.Formaner
            .Include(f => f.Ansokningar)
            .Select(f => new
            {
                namn = f.Namn,
                kostnad = f.Kostnad,
                aktiv = f.Aktiv,
                antalPersoner = f.Ansokningar.Count(a => a.Beviljad)
            })
            .ToList();

        var totalKostnad = _databas.Formaner
            .Where(f => f.Aktiv)
            .Sum(f => f.Kostnad);

        var antalAktiva = _databas.Formaner
            .Count(f => f.Aktiv);

        return new
        {
            totalKostnad = totalKostnad,
            antalAktivaFormaner = antalAktiva,
            kostnadPerForman = kostnadPerForman
        };
    }
}