using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class PayrollRepository : IRepository<Payroll>
{
    HRContext _db =  new HRContext();
    
    public Payroll GetById(int payrollId)
    {
        return _db.Payrolls.FirstOrDefault(p => p.Id == payrollId);
    }

    public List<Payroll> GetAll()
    {
        return _db.Payrolls.ToList();
    }

    public Payroll Add(Payroll payroll)
    {
        _db.Payrolls.Add(payroll);
        _db.SaveChanges();
        return payroll;
    }

    public Payroll Update(Payroll payroll)
    {
        _db.Payrolls.Update(payroll);
        _db.SaveChanges();
        return payroll;
    }

    public void Delete(int payrollId)
    {
        _db.Payrolls.Remove(_db.Payrolls.FirstOrDefault(u => u.Id == payrollId));
        _db.SaveChanges();
    }
}