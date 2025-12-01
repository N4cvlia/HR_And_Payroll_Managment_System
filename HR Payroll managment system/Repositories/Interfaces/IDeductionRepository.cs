using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IDeductionRepository
{
    List<Deduction> GetAll();
    Deduction Add(Deduction deduction);
    Deduction Update(Deduction deduction);
    void Delete(int deductionId);
}