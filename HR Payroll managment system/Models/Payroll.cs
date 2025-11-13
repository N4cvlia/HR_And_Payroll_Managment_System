using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class Payroll : Base
{
    public int EmployeeId { get; set; }
    public EmployeeProfile Employee { get; set; }
    public DateTime PayPeriodStartDate { get; set; }
    public DateTime PayPeriodEndDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal TotalBonus { get; set; }
    public decimal TotalDeduction { get; set; }
    public decimal NetSalary => BaseSalary + TotalBonus -  TotalDeduction;
    public bool IsProcessed { get; set; } = false;
    public string PayslipPath { get; set; }
    
    public List<Bonus>  Bonuses { get; set; }
    public List<Deduction> Deductions { get; set; }
}