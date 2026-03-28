namespace SOS100.Models;

public class RapportPost
{
    public int Id { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Avdelning { get; set; } = string.Empty;
    public decimal TotalKostnad { get; set; }
    public int AntalAktivaFormaner { get; set; }
    public DateTime SkapadDatum { get; set; } = DateTime.UtcNow;
    public string Kommentar { get; set; } = string.Empty;
}