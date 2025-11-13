using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class Bonus : Base
{
    public int PayrollId { get; set; }
    public Payroll Payroll { get; set; }
    public string BonusType { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
}