using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class User : Base
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public EmployeeProfile EmployeeProfile { get; set; }
}