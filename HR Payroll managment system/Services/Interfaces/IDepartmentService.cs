using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Services.Interfaces;

public interface IDepartmentService
{
    List<Department> GetAllDepartments();
    List<DepartmentSalaryReportDto> GetAllDepartmentSalaryReports();
    Department GetDepartmentById(int id);
    Department GetDepartmentByIdWithDetails(int id);
    ValidationResult AddDepartment(Department department);
    ValidationResult UpdateDepartment(Department department, Department updatedDepartment);
    
}