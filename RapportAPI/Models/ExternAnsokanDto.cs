namespace RapportAPI.Models;

public class ExternAnsokanDto
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string BenefitName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}