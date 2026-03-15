namespace status_formaner_API.Models;

public class  FormanerStatus
{
    public int ID { get; set; }
    public string EmployeeName { get; set; }
    public string FormanerTitle { get; set; }
    public string Status { get; set; } // e.g. "Active", "Pending", "Rejected"
}
