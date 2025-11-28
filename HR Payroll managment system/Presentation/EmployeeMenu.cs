using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation.Interfaces;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class EmployeeMenu : IEmployeeMenu
{
    EmployeeService _employeeService;
    AttendanceRecordService _attendanceRecordService;
    LeaveRequestService _leaveRequestService;
    PayrollService _payrollService;
    
    UserService  _userService;

    public EmployeeMenu(
        UserService userService,
        EmployeeService employeeService,
        AttendanceRecordService attendanceRecordService,
        LeaveRequestService leaveRequestService,
        PayrollService payrollService
        )
    {
        _userService = userService;
        _employeeService = employeeService;
        _attendanceRecordService = attendanceRecordService;
        _leaveRequestService = leaveRequestService;
        _payrollService = payrollService;
    }
    
    // Menu Fucntions
    #region Menu Functions
    
    public void MainMenu()
    {
        Console.Clear();
        bool isEmployeeMenuRunning = true;
        do
        {
            Console.WriteLine("=== Employee Dashboard ===");
            Console.WriteLine("1. My Profile");
            Console.WriteLine("2. Clock In/Out");
            Console.WriteLine("3. My Attendance");
            Console.WriteLine("4. Request Leave");
            Console.WriteLine("5. My Payslips");
            Console.WriteLine("6. Logout");
            Console.WriteLine("Choose an Option:");
        
            string choice =  Console.ReadLine();

            switch (choice)
            {
                case "1":
                    MyProfileMenu();
                    break;
                case "2":
                    ClockInMenu();
                    break;
                case "3":
                    MyAttendanceMenu();
                    break;
                case "4":
                    RequestLeaveMenu();
                    break;
                case "5":
                    MyPayslipsMenu();
                    break;
                case "6":
                    isEmployeeMenuRunning = false;
                    _userService.CurrentUser = null;
                    
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully Logged Out!");
                    Console.ResetColor();
                    break;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input!");
                    Console.ResetColor();
                    break;
            }
        } while (isEmployeeMenuRunning);
    }

    public void MyProfileMenu()
    {
        Console.Clear();
        
        var employee = _employeeService.GetAllEmployeesWithDetails().FirstOrDefault(e => e.Id == _userService.CurrentUser.EmployeeProfile.Id);
        
        if (employee != null)
        {
            Console.Clear();
            Console.WriteLine("=== My Profile ===");
            Console.WriteLine($"ID: {employee.Id}");
            Console.WriteLine($"NAME: {employee.FirstName} {employee.LastName}");
            Console.WriteLine($"EMAIL: {employee.Email}");
            Console.WriteLine($"DEPT: {employee.DepartmentName}");
            Console.WriteLine($"ROLE: {employee.JobPositionName}");
            Console.WriteLine($"SALARY: ${employee.BaseSalary:N0}");
            Console.WriteLine($"STATUS: {(employee.IsActive ? "ACTIVE" : "INACTIVE")}");

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    public void ClockInMenu()
    {
        bool isRunning = true;
            Console.Clear();
            do
            {
                Console.WriteLine("=== Clock In/Out ===");
                Console.WriteLine("1. Clock In");
                Console.WriteLine("2. Clock Out");
                Console.WriteLine("3. Back to Dashboard");
                Console.WriteLine("Choose An Option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();

                        var result = _attendanceRecordService.CheckIn();

                        if (result)
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successfully Checked In!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You Already Checked In Today!");
                            Console.ResetColor();
                        }

                        break;
                    case "2":
                        Console.Clear();

                        var result2 = _attendanceRecordService.CheckOut();

                        if (result2)
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successfully Checked Out!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You Already Checked Out Today Or Haven't Checked In!");
                            Console.ResetColor();
                        }
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

    public void MyAttendanceMenu()
    {
        var employee = _employeeService.GetEmployeeByIdWithAttendace(_userService.CurrentUser.EmployeeProfile.Id);
        Console.Clear();
        Console.WriteLine("=== View Attendance Records ===");
        Console.WriteLine("ID  EMPLOYEE         DATE       IN      OUT     HOURS");
        Console.WriteLine("-----------------------------------------------------");

        
        if (!employee.AttendanceRecords.Any())
        {
            Console.WriteLine("No attendance records found!");
            Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
            Console.WriteLine("Click Any Key To Exit.");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        foreach (var record in employee.AttendanceRecords.Take(20)) // Show first 20 only
            Console.WriteLine(
                $"{record.Id,-3} {record.Employee.FirstName,-15} {record.WorkDate:MM/dd}  {record.CheckIn:HH:mm}   {record.CheckOut:HH:mm}    {record.HoursWorked:F1}");

        if (employee.AttendanceRecords.Count > 20)
            Console.WriteLine($"... and {employee.AttendanceRecords.Count - 20} more records");
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    public void RequestLeaveMenu()
    {
        Console.Clear();
        Console.WriteLine("=== REQUEST LEAVE ===");
        
        Console.Write("Start Date (mm/dd/yyyy): ");
        string startInput = Console.ReadLine();
    
        Console.Write("End Date (mm/dd/yyyy): ");
        string endInput = Console.ReadLine();
    
        Console.Clear();
        Console.WriteLine("=== REQUEST LEAVE ===");
        Console.WriteLine("1. Vacation");
        Console.WriteLine("2. Sick Leave");
        Console.WriteLine("3. Personal");
        Console.WriteLine("4. Emergency");
        Console.Write("Choose A Leave Type: ");
        string typeChoice = Console.ReadLine();
        
        Console.Clear();
        Console.WriteLine("=== REQUEST LEAVE ===");
        Console.Write("Reason: ");
        string reason = Console.ReadLine();
        
        if (!DateTime.TryParse(startInput, out DateTime startDate) ||
            !DateTime.TryParse(endInput, out DateTime endDate))
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date format!");
            Console.ResetColor();
            return;
        }
        
        string leaveType = typeChoice switch
        {
            "1" => "Vacation",
            "2" => "Sick Leave",
            "3" => "Personal",
            "4" => "Emergency",
            _ => "Other"
        };
        
        Console.Clear();
        Console.WriteLine("=== LEAVE REQUEST SUMMARY ===");
        Console.WriteLine($"Type: {leaveType}");
        Console.WriteLine($"From: {startDate:MMM dd, yyyy}");
        Console.WriteLine($"To: {endDate:MMM dd, yyyy}");
        Console.WriteLine($"Days: {(endDate - startDate).Days + 1}");
        Console.WriteLine($"Reason: {reason}");
    
        Console.WriteLine("\nSubmit this request? (y/n): ");

        if (Console.ReadLine().ToLower() == "y")
        {
            
            LeaveRequest leaveRequest =  new LeaveRequest()
            {
                EmployeeId = _userService.CurrentUser.EmployeeProfile.Id,
                StartDate = startDate,
                EndDate = endDate,
                LeaveType = leaveType,
                Reason = reason,
            };
            
            var result = _leaveRequestService.AddLeaveRequest(leaveRequest);

            if (result.Success)
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully submitted Leave Request!");
                Console.ResetColor();
            }
            else
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var e in result.Errors)
                {
                    Console.WriteLine(e);
                }
                Console.ResetColor();
            }
        }
        else
        {
            Console.Clear();
        }
        
    }

    public void MyPayslipsMenu()
    {
        bool isRunning = true;
        Console.Clear();

        do
        {
            Console.WriteLine("=== My Payslips ===");
            Console.WriteLine("1. View All Payslips");
            Console.WriteLine("2. View Latest Payslip");
            Console.WriteLine("3. Download Payslip");
            Console.WriteLine("4. Back To Dashboard");
            Console.WriteLine("Choose An Option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    var payslips = _payrollService.GetPayrolls();
                    
                    Console.Clear();
                    Console.WriteLine("=== My Payslips ===");
                    Console.WriteLine("MONTH/YEAR    GROSS PAY    NET PAY    STATUS");
                    Console.WriteLine("--------------------------------------------");
    
                    foreach (var slip in payslips.OrderByDescending(p => p.PaymentDate))
                    {
                        Console.WriteLine($"{slip.PaymentDate:MMM yyyy}    ${slip.BaseSalary,8:N0}   ${slip.NetSalary,8:N0}   {slip.IsProcessed}");
                    }
    
                    if (!payslips.Any())
                    {
                        Console.WriteLine("No payslips found");
                    }
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "2":
                    var payslip = _payrollService.GetLatestPayroll(); 
                    var currentUser = _userService.CurrentUser;
                    Console.Clear();

                    if (payslip == null)
                    {
                        Console.WriteLine("=== My Payslips ===");
                        Console.WriteLine("No Payslips available");
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
                    
                    Console.WriteLine("=== PAYSLIP DETAILS ===");
                    Console.WriteLine($"Pay Period: {payslip.PaymentDate:MMMM yyyy}");
                    Console.WriteLine($"Employee: {currentUser.EmployeeProfile.FirstName + " " +  currentUser.EmployeeProfile.LastName}");
                    Console.WriteLine($"ID: {payslip.EmployeeId}");
                    Console.WriteLine($"Department: {currentUser.EmployeeProfile.Department.DepartmentName}");
                    Console.WriteLine($"Position: {currentUser.EmployeeProfile.JobPosition.PositionTitle}");
                    Console.WriteLine("==========================================");
                    
                    Console.WriteLine("EARNINGS:");
                    Console.WriteLine($"  Basic Salary:       ${payslip.BaseSalary,12:N2}");
                    
                    foreach (var bonus in payslip.Bonuses)
                    {
                        Console.WriteLine($"  {bonus.BonusType}:         ${bonus.Amount,12:N2}");
                    }
                    
                    Console.WriteLine("  -------------------------------");
                    
                    Console.WriteLine("DEDUCTIONS:");
                    
                    foreach (var deduction in payslip.Deductions)
                    {
                        Console.WriteLine($"  {deduction.DeductionType}:    -${deduction.Amount,11:N2}");
                    }
                    
                    Console.WriteLine("  -------------------------------");
                    Console.WriteLine($"  TOTAL DEDUCTIONS:   -${payslip.Deductions.Sum(d => d.Amount),11:N2}");
                    Console.WriteLine();
    
                    // SUMMARY
                    Console.WriteLine("SUMMARY:");
                    Console.WriteLine("  -------------------------------");
                    Console.WriteLine($"  NET PAY:            ${payslip.NetSalary,12:N2}");
                    Console.WriteLine("==========================================");
                    Console.WriteLine($"Payment Date: {payslip.PaymentDate:MMM dd, yyyy}");
                    Console.WriteLine($"Status: {payslip.IsProcessed}");

                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "3":
                    var currentUser2 = _userService.CurrentUser;
                    var payslips2 = _payrollService.GetPayrolls().Where(p => p.EmployeeId == currentUser2.EmployeeProfile.Id);
                    
                    Console.Clear();
                    Console.WriteLine("=== My Payslips ===");
                    Console.WriteLine("Id    MONTH/YEAR    GROSS PAY    NET PAY    STATUS");
                    Console.WriteLine("------------------------------------------------");
    
                    foreach (var slip in payslips2.OrderByDescending(p => p.PaymentDate))
                    {
                        Console.WriteLine($"${slip.Id,8:N0}   {slip.PaymentDate:MMM yyyy}    ${slip.BaseSalary,8:N0}   ${slip.NetSalary,8:N0}   {slip.IsProcessed}");
                    }
    
                    if (!payslips2.Any())
                    {
                        Console.WriteLine("No payslips found");
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Choose A Payslip Id:");

                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input!");
                        Console.ResetColor();
                        break;
                    }

                    var chosenPayslip = _payrollService.GetPayrollById(id);

                    if (chosenPayslip == null)
                    {
                        Console.Clear();
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No Payslip Found");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();
                        
                        _payrollService.ExportPayslipPDF(chosenPayslip);
                    }
                    
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