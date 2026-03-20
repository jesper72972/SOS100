namespace status_formaner_API.Models;

public class ServiceStatus
{
    public int ID { get; set; }
    public int ServicID { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

