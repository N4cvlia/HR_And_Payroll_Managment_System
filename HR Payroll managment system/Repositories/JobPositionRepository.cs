using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HR_Payroll_managment_system.Repositories;

public class JobPositionRepository : IRepository<JobPosition>
{
    HRContext _db = new HRContext();
    
    public JobPosition GetById(int jobPositionId)
    {
        return _db.JobPositions.FirstOrDefault(u => u.Id == jobPositionId);
    }

    public JobPosition GetByIdWithDetails(int jobPositionId)
    {
        return _db.JobPositions
            .Include(j => j.Employees)
            .ThenInclude(e => e.User)
            .Include(j => j.Department)
            .FirstOrDefault(j => j.Id == jobPositionId);
    }

    public List<JobPosition> GetAll()
    {
        return _db.JobPositions.Include(j => j.Department).ToList();
    }

    public JobPosition Add(JobPosition jobPosition)
    {
        _db.JobPositions.Add(jobPosition);
        _db.SaveChanges();
        return jobPosition;
    }

    public JobPosition Update(JobPosition jobPosition)
    {
        _db.JobPositions.Update(jobPosition);
        _db.SaveChanges();
        return jobPosition;
    }

    public void Delete(int jobPositionId)
    {
        _db.JobPositions.Remove(_db.JobPositions.FirstOrDefault(u => u.Id == jobPositionId));
        _db.SaveChanges();
    }
}