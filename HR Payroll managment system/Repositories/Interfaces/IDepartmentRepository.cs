using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Department GetById(int departmentId);
    Department GetByIdWithDetails(int departmentId);
    Department GetByIdWithJobPositions(int departmentId);
    List<DepartmentSalaryReportDto> GetAllWithSalaryReport();
    List<DepartmentListDto> GetAllDepartmentsWIthCount();
    List<Department> GetAllDepartmentsWithEmployeeAndAttendance();
    List<Department> GetAll();
    Department Add(Department department);
    Department Update(Department department);
    void Delete(int departmentId);
}