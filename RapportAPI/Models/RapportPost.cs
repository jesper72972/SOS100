namespace RapportAPI.Models;

public class RapportPost
{
    public int Id { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Beskrivning { get; set; } = string.Empty;
    public DateTime Skapad { get; set; } = DateTime.UtcNow;
    public string SkapadAv { get; set; } = string.Empty;
}