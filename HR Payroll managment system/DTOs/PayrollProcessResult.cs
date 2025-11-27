using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.DTOs;

public class PayrollProcessResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int ProcessedCount { get; set; }
    public decimal TotalGrossPay { get; set; }
    public decimal TotalNetPay { get; set; }
    public string PayPeriod { get; set; }
    public List<Payroll> Payrolls { get; set; }
}