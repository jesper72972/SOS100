namespace GodkannadeAPI.Models;

public class Approval
{
    public int Id { get; set; }

    // ID på ansökan som godkänns
    public int ApplicationId { get; set; }

    // ID på personen som godkänner
    public int ApproverId { get; set; }

    // Approved eller Rejected
    public string Decision { get; set; } = string.Empty;

    // Kommentar från chefen
    public string Comment { get; set; } = string.Empty;

    // Datum för beslutet
    public DateTime DecisionDate { get; set; } = DateTime.UtcNow;
}