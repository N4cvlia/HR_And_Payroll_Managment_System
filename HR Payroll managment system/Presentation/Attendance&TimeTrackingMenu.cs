using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class Attendance_TimeTrackingMenu : IAttendanceRecordService
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
            Console.WriteLine("=== ATTENDANCE & TIME TRACKING ===");
            Console.WriteLine("1. Clock In/Out");
            Console.WriteLine("2. View Attendance Records");
            Console.WriteLine("3. Manage Leave Requests");
            Console.WriteLine("4. Generate Timesheets");
            Console.WriteLine("5. View Attendance Reports");
            Console.WriteLine("6. Back to HR Menu");
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

    public void ClockInMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Clock In/Out ===");

        var allEmployees = _employeeService.GetAllEmployeesWithDetails();

        Console.Clear();

        Console.WriteLine("=== View All Employees ===");
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
                            $"{record.Id,-3} {record.Employee.FirstName,-15} {record.WorkDate:MM/dd}  {record.CheckIn:HH:mm}   {record.CheckOut:HH:mm}    {record.HoursWorked:F1}");

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
                            $"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");

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
                                $"{record.Id,-3} {record.Employee.FirstName,-15} {record.WorkDate:MM/dd}  {record.CheckIn:HH:mm}   {record.CheckOut:HH:mm}    {record.HoursWorked:F1}");

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

    #endregion
}