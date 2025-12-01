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
                case "2":
                    GeneratePayslipsMenu();
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
                    var employee = _employeeService.GetEmployeeByIdWithFullDetails(payroll.EmployeeId);
                    _pdfHelper.ExportToPDFMonthly(payroll, _userService.CurrentUser, employee);
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

    public void GeneratePayslipsMenu()
    {
        Console.Clear();
        var allEmployees = _employeeService.GetAllEmployeesWithDetails();
        Console.WriteLine("=== Generate Payslips ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
        foreach (var emp in allEmployees)
        {
            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
        }
    
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Enter an employee id:");
        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }
        
        var employee = _employeeService.GetEmployeeByIdWithFullDetails(employeeId);

        if (employee == null)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Employee not found!");
            Console.ResetColor();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("=== Generate Payslips ===");
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
            
            var result = _payrollService.ProcessMonthlyPayrollForSingle(year,month,employee);

            if (result.Success)
            {
                _pdfHelper.ExportToPDFMonthly(result.payroll, _userService.CurrentUser, employee);
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Exported Payslips To PDF");
                Console.ResetColor();
            }
            else
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Message);
                Console.ResetColor();
            }
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

        if (!int.TryParse(Console.ReadLine(), out int id))
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

            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
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
        
        if (!int.TryParse(Console.ReadLine(), out int id))
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

            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
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
            Console.WriteLine("2. Search by Date");
            Console.WriteLine("3. Back To HR Menu");
            Console.WriteLine("Choose An Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    var payrolls = _payrollService.GetPayrollsWithDetails();
                    Console.Clear();

                    Console.WriteLine("=== Payroll History ===");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("ID  EMPLOYEE              PERIOD       GROSS     NET       STATUS    PROCESSED DATE");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");

                    if (!payrolls.Any())
                    {
                        Console.WriteLine("No payroll runs found.");
                        Console.WriteLine(
                            "───────────────────────────────────────────────────────────────────────────────");
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }

                    // Find the longest employee name for proper spacing
                    int maxNameLength = payrolls
                        .Where(p => p.Employee != null)
                        .Max(p => (p.Employee.FirstName + " " + p.Employee.LastName).Length);
                    var nameColumnWidth = Math.Max(maxNameLength, 8) + 2;

                    foreach (var payroll in payrolls.OrderByDescending(p => p.CreatedAt))
                    {
                        var employeeName = "Unknown";
                        if (payroll.Employee != null)
                            employeeName = $"{payroll.Employee.FirstName} {payroll.Employee.LastName}";
                        else
                            employeeName = $"ID:{payroll.EmployeeId}";

                        var status = payroll.IsProcessed ? "PAID" : "PENDING";
                        var statusColor = payroll.IsProcessed ? "✅" : "⏳";

                        Console.WriteLine(
                            $"{payroll.Id,-3} {employeeName.PadRight(nameColumnWidth)} {payroll.PayPeriodStartDate:MM/yyyy}  {payroll.BaseSalary + payroll.TotalBonus,7:N0}$ {payroll.NetSalary,7:N0}$        {status,-7} {payroll.CreatedAt:MM/dd/yyyy}");
                    }

                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");

                    // Summary
                    var totalGross = payrolls.Sum(p => p.BaseSalary + p.TotalBonus);
                    var totalNet = payrolls.Sum(p => p.NetSalary);
                    var paidCount = payrolls.Count(p => p.IsProcessed);
                    var pendingCount = payrolls.Count(p => !p.IsProcessed);

                    Console.WriteLine(
                        $"TOTAL: {payrolls.Count} payroll runs | Paid: {paidCount} | Pending: {pendingCount}");
                    Console.WriteLine($"TOTAL GROSS: ${totalGross:N0} | TOTAL NET: ${totalNet:N0}");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "2":
                    Console.Clear();

                    Console.WriteLine("=== Payroll History ===");
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
                        break;
                    }

                    var payrolls2 = _payrollService.GetByDate(year, month);

                    Console.Clear();
                    Console.WriteLine("=== Payroll History ===");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("ID  EMPLOYEE              PERIOD       GROSS     NET       STATUS    PROCESSED DATE");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");

                    if (!payrolls2.Any())
                    {
                        Console.WriteLine("No payroll runs found.");
                        Console.WriteLine(
                            "───────────────────────────────────────────────────────────────────────────────");
                        Console.WriteLine("Press any key to exit...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }

                    // Find the longest employee name for proper spacing
                    int maxNameLength2 = payrolls2
                        .Where(p => p.Employee != null)
                        .Max(p => (p.Employee.FirstName + " " + p.Employee.LastName).Length);
                    var nameColumnWidth2 = Math.Max(maxNameLength2, 8) + 2;

                    foreach (var payroll in payrolls2.OrderByDescending(p => p.CreatedAt))
                    {
                        var employeeName = "Unknown";
                        if (payroll.Employee != null)
                            employeeName = $"{payroll.Employee.FirstName} {payroll.Employee.LastName}";
                        else
                            employeeName = $"ID:{payroll.EmployeeId}";

                        var status = payroll.IsProcessed ? "PAID" : "PENDING";
                        var statusColor = payroll.IsProcessed ? "✅" : "⏳";

                        Console.WriteLine(
                            $"{payroll.Id,-3} {employeeName.PadRight(nameColumnWidth2)} {payroll.PayPeriodStartDate:MM/yyyy}  {payroll.BaseSalary + payroll.TotalBonus,7:N0}$ {payroll.NetSalary,7:N0}$        {status,-7} {payroll.CreatedAt:MM/dd/yyyy}");
                    }

                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");

                    // Summary
                    var totalGross2 = payrolls2.Sum(p => p.BaseSalary + p.TotalBonus);
                    var totalNet2 = payrolls2.Sum(p => p.NetSalary);
                    var paidCount2 = payrolls2.Count(p => p.IsProcessed);
                    var pendingCount2 = payrolls2.Count(p => !p.IsProcessed);

                    Console.WriteLine(
                        $"TOTAL: {payrolls2.Count} payroll runs | Paid: {paidCount2} | Pending: {pendingCount2}");
                    Console.WriteLine($"TOTAL GROSS: ${totalGross2:N0} | TOTAL NET: ${totalNet2:N0}");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "3":
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