public class Rapport
{
    public int Id { get; set; }
    public string? Formanansokan { get; set; }
    public string? Status { get; set; }
    public string? Avdelning { get; set; }
    public DateTime SkapadDatum { get; set; } = DateTime.Now;
}