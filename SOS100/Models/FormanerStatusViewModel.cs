namespace SOS100.Models;

public class FormanerStatusViewModel
{
    public ServiceStatus[] Statuses { get; set; } = Array.Empty<ServiceStatus>();
    public Comment[] Comments { get; set; } = Array.Empty<Comment>();
    public Formaner[] Formaners { get; set; } = Array.Empty<Formaner>();
    public string? SelectedName { get; set; }
    
    public IEnumerable<string> AvailableNames { get; set; } = Enumerable.Empty<string>();
    

}
