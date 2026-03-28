namespace SOS100.Models;

public class ManagerApprovalItem
{
    public int StatusId { get; set; }
    public int ApplicationId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;
    public string BenefitName { get; set; } = string.Empty;
    public string ApplicationMessage { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public string AdminComment { get; set; } = string.Empty;
    public string UserComment { get; set; } = string.Empty;

    public List<CommentApiItem> AllComments { get; set; } = new();
    public bool HasUnreadUserComment { get; set; }
}

public class ApplicationApiItem
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string BenefitName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
}

public class CommentApiItem
{
    public int ID { get; set; }
    public int StatusOBJ { get; set; }
    public string? AdminComment { get; set; }
    public string? UserCommemt { get; set; }
}

public class ServiceStatusApiItem
{
    public int ID { get; set; }
    public int ServicID { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}