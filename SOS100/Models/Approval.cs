using System.ComponentModel.DataAnnotations;

namespace SOS100.Models;

public class Approval
{
    public int Id { get; set; }

    [Required]
    public int ApplicationId { get; set; }

    [Required]
    public int ApproverId { get; set; }

    [Required]
    public string Decision { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;

    public DateTime DecisionDate { get; set; }
}