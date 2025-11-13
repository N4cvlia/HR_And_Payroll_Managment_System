using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class Role : Base
{
    public string RoleName { get; set; }
    public List<User> Users { get; set; }
}