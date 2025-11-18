using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;
using HR_Payroll_managment_system.Validators;

namespace HR_Payroll_managment_system.Services;

public class DepartmentService : IDepartmentService
{
    DepartmentRepository _departmentRepository =  new DepartmentRepository();
    ActivityLogsRepository _activityLogsRepository = new ActivityLogsRepository();
    
    DepartmentValidator _departmentValidator = new DepartmentValidator();
    
    UserService _userService;
    
    Logging _logging = new Logging();

    public DepartmentService(UserService userService)
    {
        _userService = userService;
    }

    public List<Department> GetAllDepartments()
    {
        return _departmentRepository.GetAll();
    }

    public List<DepartmentSalaryReportDto> GetAllDepartmentSalaryReports()
    {
        return _departmentRepository.GetAllWithSalaryReport();
    }

    public Department GetDepartmentById(int id)
    {
        return _departmentRepository.GetById(id);
    }

    public Department GetDepartmentByIdWithDetails(int id)
    {
        return  _departmentRepository.GetByIdWithDetails(id);
    }

    public ValidationResult AddDepartment(Department department)
    {
        var isValidDepartment = _departmentValidator.Validate(department);

        if (!isValidDepartment.IsValid)
            return new ValidationResult
            {
                Success = false,
                Errors = isValidDepartment.Errors.Select(e => e.ErrorMessage).ToList()
            };

        var currentUser = _userService.CurrentUser;

        _departmentRepository.Add(department);

        var activityLog = new ActivityLog
        {
            UserId = currentUser.Id,
            Action = "Department Creation",
            Description = $"{department.DepartmentName} Was created"
        };

        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsRepository.Add(activityLog);

        return new ValidationResult { Success = true };
    }

    public ValidationResult UpdateDepartment(Department department, Department updatedDepartment)
    {
        var isValidDepartment = _departmentValidator.Validate(updatedDepartment);

        if (!isValidDepartment.IsValid)
            return new ValidationResult
            {
                Success = false,
                Errors = isValidDepartment.Errors.Select(e => e.ErrorMessage).ToList()
            };

        var currentUser = _userService.CurrentUser;

        department.DepartmentName = updatedDepartment.DepartmentName;
        department.Description = updatedDepartment.Description;

        _departmentRepository.Update(department);
        
        ActivityLog activityLog = new ActivityLog()
        {
            UserId = currentUser.Id,
            Action = "Department Edited",
            Description = $"{department.DepartmentName} Was Edited"
        };
        
        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsRepository.Add(activityLog);
        return new ValidationResult { Success = true };
    }
}