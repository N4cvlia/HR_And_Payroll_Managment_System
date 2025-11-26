using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HR_Payroll_managment_system.Repositories;

public class EmployeeRepository : IRepository<EmployeeProfile>
{
    HRContext _db = new HRContext();
    
    public EmployeeProfile GetById(int employeeId)
    {
        return _db.Employees.FirstOrDefault(u => u.Id == employeeId);
    }

    public EmployeeProfile GetByIdWithDetails(int id)
    {
        return _db.Employees
            .Include(e => e.Department)
            .Include(e => e.AttendanceRecords)
            .Include(e => e.User)
            .FirstOrDefault(u => u.Id == id);
    }

    public EmployeeProfile GetByIdWithPayrolls(int id)
    {
        return _db.Employees
            .Include(e => e.Payrolls)
            .FirstOrDefault(e => e.Id == id);
    }

    public EmployeeProfile GetByIdWithAttendace(int id)
    {
        return _db.Employees.Include(e => e.AttendanceRecords).FirstOrDefault(u => u.Id == id);
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
                BaseSalary = e.BaseSalary,
                Email = e.User.Email,
                IsActive = e.IsActive
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