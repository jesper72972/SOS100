using System.ComponentModel.DataAnnotations;

namespace GodkannadeAPI.Models;

public class Approval
{
    public int Id { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ApplicationId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ApproverId { get; set; }

    [Required]
    [RegularExpression("Approved|Rejected", ErrorMessage = "Decision must be Approved or Rejected")]
    public string Decision { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Comment { get; set; } = string.Empty;

    public DateTime DecisionDate { get; set; } = DateTime.UtcNow;
}