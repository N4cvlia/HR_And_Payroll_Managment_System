using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;

namespace HR_Payroll_managment_system.Services;

public class PayrollService : IPayrollService
{
    PayrollRepository  _payrollRepository = new PayrollRepository();

    private UserService _userService;

    public PayrollService(UserService userService)
    {
        _userService = userService;
    }

    public List<Payroll> GetPayrolls()
    {
        return _payrollRepository.GetAll();
    }

    public Payroll GetLatestPayroll()
    {
        return _payrollRepository.GetAll().LastOrDefault();
    }
}