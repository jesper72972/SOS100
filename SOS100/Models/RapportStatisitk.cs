namespace SOS100.Models;

public class RapportStatistik
{
    public decimal TotalKostnad { get; set; }
    public int AntalAktivaFormaner { get; set; }
    public RapportForman[] KostnadPerForman { get; set; } = Array.Empty<RapportForman>();
}

public class RapportForman
{
    public string Namn { get; set; } = string.Empty;
    public decimal Kostnad { get; set; }
    public bool Aktiv { get; set; }
    public int AntalPersoner { get; set; }
}