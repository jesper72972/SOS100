namespace SOS100.Models;

public class Approval
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public int ApproverId { get; set; }
    public string Decision { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime DecisionDate { get; set; }
}