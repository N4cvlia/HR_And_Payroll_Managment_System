namespace HR_Payroll_managment_system.Models;

public class ValidationResult
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}