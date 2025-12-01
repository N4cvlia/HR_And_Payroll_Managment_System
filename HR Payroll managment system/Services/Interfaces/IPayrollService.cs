using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;

namespace HR_Payroll_managment_system.Services.Interfaces;

public interface IPayrollService
{
    void AddPayroll(Payroll payroll);
    void AddRange(List<Payroll> payrolls);
    List<Payroll> GetByDate(int year, int month);
    List<Payroll> GetPayrolls();
    List<Payroll> GetPayrollsWithDetails();
    Payroll GetLatestPayroll();
    Payroll GetPayrollById(int id);
    void ExportPayslipPDF(Payroll payroll);
    PayrollProcessResult ProcessMonthlyPayroll(int year, int month);
    PayrollProcessResultSingle ProcessMonthlyPayrollForSingle(int year, int month, EmployeeProfile employee);
}