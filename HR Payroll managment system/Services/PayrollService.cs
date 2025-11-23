using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;

namespace HR_Payroll_managment_system.Services;

public class PayrollService : IPayrollService
{
    PayrollRepository  _payrollRepository = new PayrollRepository();

    private UserService _userService;
    
    PDFHelper _pdfHelper = new PDFHelper();

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

    public Payroll GetPayrollById(int id)
    {
        return _payrollRepository.GetById(id);
    }

    public void ExportPayslipPDF(Payroll payroll)
    {
        _pdfHelper.ExportToPDF(payroll, _userService.CurrentUser);
    }
}