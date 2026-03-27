namespace RapportAPI.Models;

public class Forman
{
    public int Id { get; set; }
    public string Namn { get; set; } = string.Empty;
    public decimal Kostnad { get; set; }
    public bool Aktiv { get; set; }

    public List<Ansokan> Ansokningar { get; set; } = new();
}