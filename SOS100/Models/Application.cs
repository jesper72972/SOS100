namespace SOS100.Models;

public class Application
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = "";
    public string BenefitName { get; set; } = "";
    public string Message { get; set; } = "";
    public string Status { get; set; } = "Pending";
}
