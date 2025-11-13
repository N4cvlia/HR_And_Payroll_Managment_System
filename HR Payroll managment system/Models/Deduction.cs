using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class Deduction : Base
{
    public int PaysrollId { get; set; }
    public Payroll Payroll { get; set; }
    public string DeductionType { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}