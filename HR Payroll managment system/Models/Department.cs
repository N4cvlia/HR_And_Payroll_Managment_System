using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class Department : Base
{
    public string DepartmentName { get; set; }
    public string Description { get; set; }
    public List<EmployeeProfile>  Employees { get; set; }
    public List<JobPosition> JobPositions { get; set; }
}