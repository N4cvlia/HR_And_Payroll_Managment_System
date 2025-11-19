using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HR_Payroll_managment_system.Repositories;

public class DepartmentRepository : IRepository<Department>
{
    HRContext _db = new HRContext();
    
    public Department GetById(int departmentId)
    {
        return _db.Departments.FirstOrDefault(u => u.Id == departmentId);
    }

    public Department GetByIdWithDetails(int departmentId)
    {
        return _db.Departments
            .Include(d => d.Employees)
            .ThenInclude(d => d.JobPosition)
            .Include(d => d.Employees)
            .ThenInclude(d => d.User)
            .FirstOrDefault(d => d.Id == departmentId);
    }

    public List<DepartmentSalaryReportDto> GetAllWithSalaryReport()
    {
        return _db.Departments.Select(d => new DepartmentSalaryReportDto
        {
            DepartmentName = d.DepartmentName,
            EmployeeCount = d.Employees.Count(e => e.IsActive),
            TotalSalary = d.Employees.Where(e => e.IsActive).Sum(e => (decimal?)e.BaseSalary) ?? 0,
            AverageSalary = d.Employees.Where(e => e.IsActive).Average(e => (decimal?)e.BaseSalary) ?? 0
        }).ToList();
    }

    public List<DepartmentListDto> GetAllDepartmentsWIthCount()
    {
        return _db.Departments
            .Select(d => new DepartmentListDto
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName,
                EmployeeCount = d.Employees.Count,
                Description = d.Description
            }).ToList<DepartmentListDto>();
    }

    public List<Department> GetAll()
    {
        return _db.Departments.Include(d => d.Employees).ToList();
    }

    public Department Add(Department department)
    {
        _db.Departments.Add(department);
        _db.SaveChanges();
        return department;
    }

    public Department Update(Department department)
    {
        _db.Departments.Update(department);
        _db.SaveChanges();
        return department;
    }

    public void Delete(int departmentId)
    {
        _db.Departments.Remove(_db.Departments.FirstOrDefault(u => u.Id == departmentId));
        _db.SaveChanges();
    }
}