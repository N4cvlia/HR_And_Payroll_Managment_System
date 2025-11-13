using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class JobPosition : Base
{
    public string PositionTitle { get; set; }
    public string Description { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public List<EmployeeProfile> Employees { get; set; }
}