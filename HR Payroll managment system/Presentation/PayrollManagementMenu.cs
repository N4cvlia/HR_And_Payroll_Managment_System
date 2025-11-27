using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation.Interfaces;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class PayrollManagementMenu : IPayrollManagementMenu
{
    EmployeeService _employeeService;
    BonusService _bonusService;
    DeductionService _deductionService;
    PayrollService _payrollService;
    UserService _userService;
    
    PDFHelper  _pdfHelper = new PDFHelper();

    public PayrollManagementMenu(EmployeeService employeeService,  BonusService bonusService, DeductionService deductionService,  PayrollService payrollService, UserService userService)
    {
        _employeeService = employeeService;
        _bonusService = bonusService;
        _deductionService = deductionService;
        _payrollService = payrollService;
        _userService = userService;
    }

    // Menu Fucntions
    #region Menu Functions

    public void MainMenu()
    {
        bool isRunning = true;
        Console.Clear();
        
        do
        {
            Console.WriteLine("=== Payroll Management ===");
            Console.WriteLine("1. Process Monthly Payroll");
            Console.WriteLine("2. Generate Pay Slips");
            Console.WriteLine("3. Bonus Management");
            Console.WriteLine("4. Deduction Management");
            Console.WriteLine("5. Payroll History");
            Console.WriteLine("6. Back to HR Menu");
            Console.WriteLine("Choose an Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    MonthlyPayrollMenu();
                    break;
                case "3":
                    BonusManagementMenu();
                    break;
                case "4":
                    DeductionManagementMenu();
                    break;
                case "5":
                    PayrollHistoryMenu();
                    break;
                case "6":
                    Console.Clear();
                    isRunning = false;
                    break;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input!");
                    Console.ResetColor();
                    break;
            }
        } while (isRunning);
    }

    public void MonthlyPayrollMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Process Monthly Payroll ===");
        Console.Write("Enter year (YYYY): ");
        string chosenYear = Console.ReadLine();
        Console.Write("Enter month (1-12): ");
        string chosenMonth = Console.ReadLine();

        if (!int.TryParse(chosenYear, out var year) ||
            !int.TryParse(chosenMonth, out var month) ||
            year < 2000 || month > 12 || month < 1)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }

        var result = _payrollService.ProcessMonthlyPayroll(year, month);

        if (result.Success)
        {
            Console.Clear();
            Console.WriteLine("=== Process Monthly Payroll ===");
            Console.WriteLine($"Processed {result.ProcessedCount} Employees");
            Console.WriteLine($"Total Net Pay: {result.TotalNetPay:N0}");

            Console.WriteLine();
            Console.WriteLine("Generate PDF payslips? (y/n):");

            if (Console.ReadLine().ToLower() == "y")
            {
                foreach (var payroll in result.Payrolls)
                {
                    _pdfHelper.ExportToPDFMonthly(payroll, _userService.CurrentUser);
                }
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Exported Payslips To PDFs");
                Console.ResetColor();
            }
        }
        else
        { 
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Message);
                Console.ResetColor();
        }
    }

    public void BonusManagementMenu()
    {
        Console.Clear();

        var allEmployees = _employeeService.GetAllEmployeesWithDetails();
    
        Console.Clear();
    
        Console.WriteLine("=== Bonus Management ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
        foreach (var emp in allEmployees)
        {
            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
        }
    
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Choose Employee Id:");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }
        
        var result = _employeeService.GetEmployeeByIdWithPayrolls(id);

        if (result == null)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Employee Found!");
            Console.ResetColor();
        }
        else
        {
            Console.Clear();
            
            Console.WriteLine("=== Bonus Management ===");
            Console.WriteLine("1. Performance Bonus");
            Console.WriteLine("2. Annual Bonus");
            Console.WriteLine("3. Sales Commission");
            Console.WriteLine("4. Referral Bonus");
            Console.WriteLine("5. Attendance Bonus");
            Console.WriteLine("6. Project Bonus");
            Console.WriteLine("7. Holiday Bonus");
            Console.WriteLine("8. Spot Bonus");
            Console.WriteLine("9. Other");
            Console.WriteLine("Choose Bonus Type:");
            
            string bonusType = Console.ReadLine() switch
            {
                "1" => "Performance Bonus",
                "2" => "Annual Bonus", 
                "3" => "Sales Commission",
                "4" => "Referral Bonus",
                "5" => "Attendance Bonus",
                "6" => "Project Bonus",
                "7" => "Holiday Bonus",
                "8" => "Spot Bonus",
                _ => "Other Bonus"
            };
            
            Console.Clear();
            
            Console.WriteLine("=== Bonus Management ===");
            Console.WriteLine("Enter Bonus Amount($):");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Enter Bonus Description:");
            string desc = Console.ReadLine();
            
            Bonus bonus =  new Bonus()
            {
                Amount = amount,
                Description = desc,
                BonusType = bonusType
            };

            var bonusResult = _bonusService.AddBonus(bonus, result);

            if (bonusResult.Success)
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Added Bonus!");
                Console.ResetColor();
            }
            else
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to Add Bonus!");
                Console.ResetColor();
            }
        }
    }

    public void DeductionManagementMenu()
    {
        Console.Clear();

        var allEmployees = _employeeService.GetAllEmployeesWithDetails();
    
        Console.Clear();
    
        Console.WriteLine("=== Deduction Management ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
        foreach (var emp in allEmployees)
        {
            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
        }
    
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Choose Employee Id:");
        
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }
        
        var result = _employeeService.GetEmployeeByIdWithPayrolls(id);

        if (result == null)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Employee Found!");
            Console.ResetColor();
        }
        else
        {
            Console.Clear();

            Console.WriteLine("=== Deduction Management ===");
            Console.WriteLine("1. Income Tax");
            Console.WriteLine("2. Federal Tax");
            Console.WriteLine("3. Social Security Tax");
            Console.WriteLine("4. Health Insurance");
            Console.WriteLine("5. Dental Insurance");
            Console.WriteLine("6. Life Insurance");
            Console.WriteLine("7. Retirement Plan");
            Console.WriteLine("8. Loan Repayment");
            Console.WriteLine("9. Union Dues");
            Console.WriteLine("10. Other");
            Console.WriteLine("Choose Deduction Type:");
            
            string deductionType = Console.ReadLine() switch
            {
                "1" => "Income Tax",
                "2" => "Federal Tax",
                "3" => "Social Security Tax",
                "4" => "Health Insurance",
                "5" => "Dental Insurance",
                "6" => "Life Insurance",
                "7" => "Retirement Plan",
                "8" => "Loan Repayment",
                "9" => "Union Dues",
                _ => "Other Deduction"
            };
            
            Console.Clear();
            
            Console.WriteLine("=== Deduction Management ===");
            Console.WriteLine("Enter Deduction Amount($):");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Enter Deduction Description:");
            string desc = Console.ReadLine();

            Deduction ded = new Deduction()
            {
                Amount = amount,
                Description = desc,
                DeductionType = deductionType
            };
            
            var deductionResult = _deductionService.AddDeduction(ded, result);

            if (deductionResult.Success)
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Added Deduction!");
                Console.ResetColor();
            }
            else
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to Add Deduction!");
                Console.ResetColor();
            }
        }
    }

    public void PayrollHistoryMenu()
    {
        Console.Clear();
        bool isRunning = true;

        do
        {
            Console.WriteLine("=== Payroll History ===");
            Console.WriteLine("1. View All Payroll Runs");
            Console.WriteLine("2. Search by Date Range");
            Console.WriteLine("3. Export Payroll Report");
            Console.WriteLine("4. Back To HR Menu");
            Console.WriteLine("Choose An Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    
                    break;
                case "4":
                    Console.Clear();
                    isRunning = false;
                    break;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input!");
                    Console.ResetColor();
                    break;
            }
        } while (isRunning);
    }

    #endregion
}