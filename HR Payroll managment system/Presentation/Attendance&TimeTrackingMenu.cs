using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Presentation.Interfaces;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class Attendance_TimeTrackingMenu : IAttendance_TimeTrackingMenu
{
    EmployeeService _employeeService;
    AttendanceRecordService _attendanceRecordService;
    
    public Attendance_TimeTrackingMenu(EmployeeService employeeService,  AttendanceRecordService attendanceRecordService)
    {
        _employeeService = employeeService;
        _attendanceRecordService = attendanceRecordService;
    }

    // Menu Fucntions

    #region Menu Functions

    public void MainMenu()
    {
        var isRunning = true;
        Console.Clear();

        do
        {
            Console.WriteLine("=== Attendance & Time Tracking ===");
            Console.WriteLine("1. Clock In/Out");
            Console.WriteLine("2. View Attendance Records");
            Console.WriteLine("3. Generate Timesheets");
            Console.WriteLine("4. View Attendance Reports");
            Console.WriteLine("5. Back to HR Menu");
            Console.WriteLine("Choose an Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    ClockInMenu();
                    break;
                case "2":
                    ViewAttendanceMenu();
                    break;
                case "3":
                    GenerateTimesheetMenu();
                    break;
                case "4":
                    ViewAttendanceReports();
                    break;
                case "5":
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

    public void ClockInMenu()
    {
        var allEmployees = _employeeService.GetAllEmployeesWithDetails();

        Console.Clear();
        Console.WriteLine("=== Clock In/Out ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");

        foreach (var emp in allEmployees)
            Console.WriteLine(
                $"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");

        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Choose Employee ID:");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }

        var employee = _employeeService.GetEmployeeById(id);

        if (employee == null)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Employee Found!");
            Console.ResetColor();
        }
        else
        {
            bool isRunning = true;
            Console.Clear();
            do
            {
                Console.WriteLine("=== Clock In/Out ===");
                Console.WriteLine("1. Clock In");
                Console.WriteLine("2. Clock Out");
                Console.WriteLine("3. Back to HR Menu");
                Console.WriteLine("Choose An Option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();

                        var result = _attendanceRecordService.CheckInAsHr(employee.Id);

                        if (result)
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successfully Checked In The Employee!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This User Already Checked In Today!");
                            Console.ResetColor();
                        }

                        break;
                    case "2":
                        Console.Clear();

                        var result2 = _attendanceRecordService.CheckOutAsHR(employee.Id);

                        if (result2)
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successfully Checked Out The Employee!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This User Already Checked Out Today Or Hasn't Checked In!");
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
    }

    public void ViewAttendanceMenu()
    {
        Console.Clear();

        var isRunning = true;

        do
        {
            Console.WriteLine("=== View Attendance Records ===");
            Console.WriteLine("1. View All Records");
            Console.WriteLine("2. Search By Employee");
            Console.WriteLine("3. Back To HR Menu");
            Console.WriteLine("Choose An Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    var attendanceRecords = _attendanceRecordService.GetAllWithEmployeeDetails();

                    Console.WriteLine("=== View Attendance Records ===");
                    Console.WriteLine("ID  EMPLOYEE         DATE       IN      OUT     HOURS");
                    Console.WriteLine("-----------------------------------------------------");

                    foreach (var record in attendanceRecords.Take(20)) // Show first 20 only
                        Console.WriteLine(
                            $"{record.Id,-3} {record.Employee.FirstName + " " + record.Employee.LastName,-15} {record.WorkDate:MM/dd}  {record.CheckIn:HH:mm}   {record.CheckOut:HH:mm}    {record.HoursWorked:F1}");

                    if (attendanceRecords.Count > 20)
                        Console.WriteLine($"... and {attendanceRecords.Count - 20} more records");
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "2":
                    Console.Clear();

                    var allEmployees2 = _employeeService.GetAllEmployeesWithDetails();

                    Console.WriteLine("=== View Attendance Records ===");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");

                    foreach (var emp in allEmployees2)
                        Console.WriteLine(
                            $"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}      {emp.DepartmentName,-9}      {emp.JobPositionName,-17} {emp.Email,-18}");

                    Console.WriteLine(
                        "───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Choose Employee ID:");
                    if (!int.TryParse(Console.ReadLine(), out var id2))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input!");
                        Console.ResetColor();
                        return;
                    }

                    var employee2 = _employeeService.GetEmployeeByIdWithAttendace(id2);

                    if (employee2 == null)
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No Employee Found!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=== View Attendance Records ===");
                        Console.WriteLine("ID  EMPLOYEE         DATE       IN      OUT     HOURS");
                        Console.WriteLine("-----------------------------------------------------");

                        foreach (var record in employee2.AttendanceRecords.Take(20)) // Show first 20 only
                            Console.WriteLine(
                                $"{record.Id,-3} {record.Employee.FirstName + " " + record.Employee.LastName,-15} {record.WorkDate:MM/dd}  {record.CheckIn:HH:mm}   {record.CheckOut:HH:mm}    {record.HoursWorked:F1}");

                        if (employee2.AttendanceRecords.Count > 20)
                            Console.WriteLine($"... and {employee2.AttendanceRecords.Count - 20} more records");
                        Console.WriteLine("-----------------------------------------------------");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
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

    public void GenerateTimesheetMenu()
    {
        var allEmployees = _employeeService.GetAllEmployeesWithDetails();
    
        Console.Clear();
    
        Console.WriteLine("=== Generate Timesheets ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
        foreach (var emp in allEmployees)
        {
            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
        }
    
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Choose Employee ID:");

        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }
        
        var employee =  _employeeService.GetEmployeeByIdWithDetails(id);

        if (employee == null)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Employee Found!");
            Console.ResetColor();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("=== Generate Timesheets ===");
            Console.Write("Pay Period Start (MM/dd/yyyy): ");
            string startInput = Console.ReadLine();
        
            Console.Write("Pay Period End (MM/dd/yyyy): ");
            string endInput = Console.ReadLine();

            if (!DateTime.TryParse(startInput, out DateTime startDate) ||
                !DateTime.TryParse(endInput, out DateTime endDate))
            {
                Console.Clear();
            
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid date format!");
                Console.ResetColor();
                return;
            }
            
            _attendanceRecordService.ExportTimesheetToPDF(employee, startDate, endDate);
        }
    }

    public void ViewAttendanceReports()
    {
        bool isRunning = true;
        Console.Clear();

        do
        {
            Console.WriteLine("=== View Attendance Reports ===");
            Console.WriteLine("1. Daily Attendance Summary");
            Console.WriteLine("2. Monthly Attendance Summary");
            Console.WriteLine("3. Back To HR Menu");
            Console.WriteLine("Choose An Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    var summary = _attendanceRecordService.GetDailyReport();
                    var totalPresent = summary.Sum(a => a.Present);
                    var totalAbsent = summary.Sum(a => a.Absent);

                    Console.Clear();
                    Console.WriteLine("=== Daily Attendance Summary ===");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("DEPARTMENT           PRESENT  ABSENT");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    
                    int maxDeptLength = summary.Max(d => d.DepartmentName.Length);
                    int deptColumnWidth = Math.Max(maxDeptLength, 10) + 2;

                    foreach (var dept in summary)
                    {
                        Console.WriteLine($"{dept.DepartmentName.PadRight(deptColumnWidth)} {dept.Present,-8} {dept.Absent,-7}");
                    }

                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine($"{"TOTAL".PadRight(deptColumnWidth)} {totalPresent,-8} {totalAbsent,-7}");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("=== Monthly Attendance Summary ===");
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
    
                    var summary2 = _attendanceRecordService.GetMonthlyReport(year, month);
                    var totalPresent2 = summary2.Sum(a => a.Present);
                    var totalAbsent2 = summary2.Sum(a => a.Absent);

                    Console.Clear();
                    Console.WriteLine($"=== Monthly Attendance Summary - {new DateTime(year, month, 1):MMMM yyyy} ===");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("DEPARTMENT           PRESENT  ABSENT");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");

                    int maxDeptLength2 = summary2.Max(d => d.DepartmentName.Length);
                    int deptColumnWidth2 = Math.Max(maxDeptLength2, 10) + 2;

                    foreach (var dept in summary2)
                    {
                        Console.WriteLine($"{dept.DepartmentName.PadRight(deptColumnWidth2)} {dept.Present,-8} {dept.Absent,-7}");
                    }

                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine($"{"TOTAL".PadRight(deptColumnWidth2)} {totalPresent2,-8} {totalAbsent2,-7}");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Press any key to continue...");
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