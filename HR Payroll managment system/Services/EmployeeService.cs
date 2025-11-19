using HR_Payroll_managment_system.Repositories;

namespace HR_Payroll_managment_system.Services;

public class EmployeeService
{
    EmployeeRepository _employeeRepository =  new EmployeeRepository();
    
    UserService _userService = new UserService();
}