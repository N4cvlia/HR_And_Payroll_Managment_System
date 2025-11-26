using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class DeductionRepository : IRepository<Deduction>
{
    HRContext _db = new HRContext();
    
    public Deduction GetById(int deductionId)
    {
        return _db.Deductions.FirstOrDefault(d => d.Id == deductionId);
    }

    public List<Deduction> GetAll()
    {
        return _db.Deductions.ToList();
    }

    public Deduction Add(Deduction deduction)
    {
        _db.Deductions.Add(deduction);
        _db.SaveChanges();
        return deduction;
    }

    public Deduction Update(Deduction deduction)
    {
        _db.Deductions.Update(deduction);
        _db.SaveChanges();
        return deduction;
    }

    public void Delete(int deductionId)
    {
        _db.Deductions.Remove(_db.Deductions.FirstOrDefault(d => d.Id == deductionId));
        _db.SaveChanges();
    }
}