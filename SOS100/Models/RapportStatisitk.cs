namespace SOS100.Models;

public class RapportStatistik
{
    public int TotaltAntalFormaner { get; set; }
    public int TotaltAntalAnsokningar { get; set; }
    public int TotaltAntalGodkanda { get; set; }
    public List<RapportForman> StatistikPerForman { get; set; } = new();
}

public class RapportForman
{
    public string Namn { get; set; } = string.Empty;
    public string Kategori { get; set; } = string.Empty;
    public int AntalAnsokningar { get; set; }
    public int AntalGodkanda { get; set; }
}