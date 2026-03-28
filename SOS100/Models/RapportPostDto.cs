namespace SOS100.Models;

public class RapportPostDto
{
    public int Id { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Beskrivning { get; set; } = string.Empty;
    public DateTime Skapad { get; set; }
    public string SkapadAv { get; set; } = string.Empty;
}