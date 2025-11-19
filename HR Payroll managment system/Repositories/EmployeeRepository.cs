using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class EmployeeRepository : IRepository<EmployeeProfile>
{
    HRContext _db = new HRContext();
    
    public EmployeeProfile GetById(int employeeId)
    {
        return _db.Employees.FirstOrDefault(u => u.Id == employeeId);
    }

    public List<EmployeeProfile> GetAll()
    {
        return _db.Employees.ToList();
    }

    public List<EmployeeListWithDetailsDto> GetAllWithDetails()
    {
        return _db.Employees
            .Select(e => new EmployeeListWithDetailsDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                DepartmentName = e.Department.DepartmentName,
                JobPositionName = e.JobPosition.PositionTitle,
                Email = e.User.Email
            })
            .ToList();
    }

    public EmployeeProfile Add(EmployeeProfile employeeProfile)
    {
        _db.Employees.Add(employeeProfile);
        _db.SaveChanges();
        return employeeProfile;
    }

    public EmployeeProfile Update(EmployeeProfile employeeProfile)
    {
        _db.Employees.Update(employeeProfile);
        _db.SaveChanges();
        return employeeProfile;
    }

    public void Delete(int employeeId)
    {
        _db.Employees.Remove(_db.Employees.FirstOrDefault(u => u.Id == employeeId));
        _db.SaveChanges();
    }
}