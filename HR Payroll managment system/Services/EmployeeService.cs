using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Validators;

namespace HR_Payroll_managment_system.Services;

public class EmployeeService
{
    EmployeeRepository _employeeRepository =  new EmployeeRepository();
    DepartmentRepository _departmentRepository = new DepartmentRepository();
    ActivityLogsRepository _activityLogsRepository = new ActivityLogsRepository();
    
    EmployeeProfileValidator _employeeProfileValidator = new EmployeeProfileValidator();
    
    UserService _userService;
    
    Logging _logging = new Logging();

    public EmployeeService(UserService userService)
    {
        _userService = userService;
    }

    public List<EmployeeProfile> GetAllEmployees()
    {
        return _employeeRepository.GetAll();
    }
    
    public List<EmployeeListWithDetailsDto> GetAllEmployeesWithDetails()
    {
        return  _employeeRepository.GetAllWithDetails();
    }

    public EmployeeProfile GetEmployeeById(int id)
    {
        return _employeeRepository.GetById(id);
    }

    public EmployeeProfile GetEmployeeByIdWithFullDetails(int id)
    {
        return _employeeRepository.GetByIdWithFullDetails(id);
    }

    public EmployeeProfile GetEmployeeByIdWithDetails(int id)
    {
        return _employeeRepository.GetByIdWithDetails(id);
    }

    public EmployeeProfile GetEmployeeByIdWithPayrolls(int id)
    {
        return  _employeeRepository.GetByIdWithPayrolls(id);
    }

    public EmployeeProfile GetEmployeeByIdWithAttendace(int id)
    {
        return _employeeRepository.GetByIdWithAttendace(id);
    }

    public bool AssignDepartmentAndPosition(EmployeeProfile employee)
    {
        var isValidEmployee = _employeeProfileValidator.Validate(employee);

        if (!isValidEmployee.IsValid)
            return false;

        var currentUser = _userService.CurrentUser;
        
        _employeeRepository.Update(employee);
        
        ActivityLog activityLog = new ActivityLog()
        {
            UserId = currentUser.Id,
            Action = "Employee Department And Job Position Assigned",
            Description = $"{employee.FirstName}'s Profile Was Assigned Department and Position"
        };
        
        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsRepository.Add(activityLog);
        return true;
    }

    public ValidationResult UpdateEmployee(EmployeeProfile employee)
    {
        var isValidEmployee = _employeeProfileValidator.Validate(employee);

        if (!isValidEmployee.IsValid)
            return new ValidationResult()
            {
                Success = false,
                Errors = isValidEmployee.Errors.Select(e => e.ErrorMessage).ToList()
            };

        var currentUser = _userService.CurrentUser;
        
        _employeeRepository.Update(employee);
        
        ActivityLog activityLog = new ActivityLog()
        {
            UserId = currentUser.Id,
            Action = "Employee Profile Edited",
            Description = $"{employee.FirstName}'s Profile Was Edited"
        };
        
        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsRepository.Add(activityLog);
        return new ValidationResult { Success = true };
    }
}