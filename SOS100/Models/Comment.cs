namespace SOS100.Models;

public class Comment
{
    public int ID { get; set; }
    public int StatusOBJ { get; set; }
    public string? AdminComment { get; set; }
    public string? UserCommemt { get; set; } // matches DB column name (typo preserved)
}
