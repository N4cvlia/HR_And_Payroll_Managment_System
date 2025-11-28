using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.DTOs;

public class PayrollProcessResultSingle
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public Payroll? payroll { get; set; }
}