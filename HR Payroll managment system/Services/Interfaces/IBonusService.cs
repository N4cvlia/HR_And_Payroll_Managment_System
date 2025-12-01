using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Services.Interfaces;

public interface IBonusService
{
    ValidationResult AddBonus(Bonus bonus, EmployeeProfile employee);
}