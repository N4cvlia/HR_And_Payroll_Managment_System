using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IEmployeeRepository
{
    EmployeeProfile GetById(int employeeId);
    EmployeeProfile GetByIdWithDetails(int id);
    EmployeeProfile GetByIdWithFullDetails(int id);
    EmployeeProfile GetByIdWithPayrolls(int id);
    EmployeeProfile GetByIdWithAttendace(int id);
    List<EmployeeProfile> GetAll();
    List<EmployeeListWithDetailsDto> GetAllWithDetails();
    List<EmployeeProfile> GetAllEmployeesWithDetails();
    EmployeeProfile Add(EmployeeProfile employeeProfile);
    EmployeeProfile Update(EmployeeProfile employeeProfile);
    void Delete(int employeeId);
}