using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;
using HR_Payroll_managment_system.DTOs;

namespace HR_Payroll_managment_system.Services;

public class PayrollService : IPayrollService
{
    PayrollRepository  _payrollRepository = new PayrollRepository();
    EmployeeRepository  _employeeRepository = new EmployeeRepository();
    BonusRepository  _bonusRepository = new BonusRepository();

    private UserService _userService;
    
    PDFHelper _pdfHelper = new PDFHelper();

    public PayrollService(UserService userService)
    {
        _userService = userService;
    }


    public void AddPayroll(Payroll payroll)
    {
        _payrollRepository.Add(payroll);
    }
    public void AddRange(List<Payroll> payrolls)
    {
        _payrollRepository.AddRange(payrolls);
    }

    public List<Payroll> GetByDate(int year, int month)
    {
        return _payrollRepository.GetAllWithDetails().Where(p => p.PaymentDate.Year == year && p.PaymentDate.Month == month).ToList();
    }
    public List<Payroll> GetPayrolls()
    {
        return _payrollRepository.GetAll();
    }

    public List<Payroll> GetPayrollsWithDetails()
    {
        return _payrollRepository.GetAllWithDetails();
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

    public PayrollProcessResult ProcessMonthlyPayroll(int year, int month)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Get all active employees with their department and job position
            var employees = _employeeRepository.GetAllEmployeesWithDetails();

            decimal totalCompanyGross = 0;
            decimal totalCompanyNet = 0;
            var processedEmployees = 0;
            var payrolls = new List<Payroll>();

            foreach (var employee in employees)
            {
                var payroll = CalculateEmployeePayroll(employee, startDate, endDate);

                if (payroll != null)
                {
                    payrolls.Add(payroll);
                    totalCompanyGross += payroll.BaseSalary + payroll.TotalBonus;
                    totalCompanyNet += payroll.NetSalary;
                    processedEmployees++;
                }
            }

            // Save all payrolls to database
            _payrollRepository.AddRange(payrolls);
            
            var filteredEmployees = employees.Where(e => e.Payrolls.Any()).ToList();

            return new PayrollProcessResult()
            {
                Success = true,
                ProcessedCount = processedEmployees,
                TotalGrossPay = totalCompanyGross,
                TotalNetPay = totalCompanyNet,
                PayPeriod = $"${month}/{year}",
                Payrolls = payrolls,
                employees = filteredEmployees
            };
        }
        catch (Exception ex)
        {
            return new PayrollProcessResult
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
    
    public PayrollProcessResultSingle ProcessMonthlyPayrollForSingle(int year, int month, EmployeeProfile employee)
    {
        try
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            var payroll = CalculateEmployeePayroll(employee, startDate, endDate);

            if (payroll != null)
            {
                AddPayroll(payroll);
            }
            
            return new PayrollProcessResultSingle()
            {
                Success = true,
                payroll = payroll
            };
        }
        catch (Exception ex)
        {
            return new PayrollProcessResultSingle()
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    private Payroll CalculateEmployeePayroll(EmployeeProfile employee, DateTime startDate, DateTime endDate)
    {
        // Base salary (monthly)
        var baseSalary = employee.BaseSalary;

        // Calculate bonuses for this period
        var bonuses = CalculateBonuses(employee, startDate, endDate);
        decimal totalBonus = bonuses.Sum(b => b.Amount);

        // Calculate deductions
        var deductions = CalculateDeductions(employee, baseSalary);
        decimal totalDeduction = deductions.Sum(d => d.Amount);

        var payroll = new Payroll
        {
            EmployeeId = employee.Id,
            PayPeriodStartDate = startDate,
            PayPeriodEndDate = endDate,
            PaymentDate = DateTime.Today,
            BaseSalary = baseSalary,
            TotalBonus = totalBonus,
            TotalDeduction = totalDeduction,
            IsProcessed = true,
            Bonuses = bonuses,
            Deductions = deductions,
            PayslipPath = "/Users/apple/Desktop/HR Payroll managment system/HR Payroll managment system/ExportFiles/Payslips"
        };

        return payroll;
    }
    
    private List<Bonus> CalculateBonuses(EmployeeProfile employee, DateTime startDate, DateTime endDate)
    {
        var bonuses = new List<Bonus>();
        
        // Get approved bonuses for this employee in the period
        var approvedBonuses = employee.Payrolls.Where(p =>
            p.CreatedAt >= startDate && p.CreatedAt <= endDate
        ).SelectMany(p => p.Bonuses ?? new List<Bonus>())
        .ToList();

        if (approvedBonuses.Any())
        {
            foreach (var bonus in approvedBonuses)
            {
                bonuses.Add(new Bonus
                {

                    BonusType = bonus.BonusType,
                    Amount = bonus.Amount,
                    Description = bonus.Description
                });
            }
        }
        
        return bonuses;
    }
    private List<Deduction> CalculateDeductions(EmployeeProfile employee, decimal baseSalary)
    {
        var deductions = new List<Deduction>();
        
        // Tax deduction (simplified) - 15%
        decimal tax = baseSalary * 0.15m;
        deductions.Add(new Deduction
        {
            DeductionType = "Income Tax",
            Amount = tax,
            Description = "Federal income tax"
        });

        // Insurance deduction - 5%
        decimal insurance = baseSalary * 0.05m;
        deductions.Add(new Deduction
        {
            DeductionType = "Health Insurance",
            Amount = insurance,
            Description = "Health insurance premium"
        });

        // Other fixed deductions from database
        var otherDeductions = employee.Payrolls.SelectMany(d => d.Deductions ?? new List<Deduction>())
            .ToList();

        foreach (var deduction in otherDeductions)
        {
            deductions.Add(new Deduction
            {
                DeductionType = deduction.DeductionType,
                Amount = deduction.Amount,
                Description = deduction.Description
            });
        }

        return deductions;
    }
}