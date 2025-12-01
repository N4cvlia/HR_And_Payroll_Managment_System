using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IPayrollRepository
{
    Payroll GetById(int payrollId);
    List<Payroll> GetAll();
    List<Payroll> GetAllWithDetails();
    Payroll Add(Payroll payroll);
    void AddRange(List<Payroll> payrolls);
    Payroll Update(Payroll payroll);
    void Delete(int payrollId);
}