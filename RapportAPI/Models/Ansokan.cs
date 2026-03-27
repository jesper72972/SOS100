namespace RapportAPI.Models;

public class Ansokan
{
    public int Id { get; set; }

    public int FormanId { get; set; }
    public Forman? Forman { get; set; }

    public string MedarbetarNamn { get; set; } = string.Empty;

    public bool Beviljad { get; set; }
}