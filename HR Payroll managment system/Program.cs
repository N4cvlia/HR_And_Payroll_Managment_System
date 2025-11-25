using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation;
using HR_Payroll_managment_system.Services;

#region Services
HRContext database =  new HRContext();

AuthService authService = new AuthService();
UserService userService = new UserService();
DepartmentService departmentService = new DepartmentService(userService);
JobPositionService jobPositionService = new JobPositionService(userService);
EmployeeService employeeService = new EmployeeService(userService);
AttendanceRecordService attendanceRecordService = new AttendanceRecordService(userService);
LeaveRequestService leaveRequestService = new LeaveRequestService(userService);
PayrollService payrollService = new PayrollService(userService);

DepartmentManagementMenu departmentManagementMenu = new DepartmentManagementMenu(departmentService);
JobPositionManagementMenu jobPositionManagementMenu = new JobPositionManagementMenu(departmentService, jobPositionService);
EmployeeManagementMenu employeeManagementMenu = new EmployeeManagementMenu(employeeService, departmentService, jobPositionService);
Attendance_TimeTrackingMenu attendanceTimeTrackingMenu = new Attendance_TimeTrackingMenu(employeeService, attendanceRecordService);
LeaveRequestManagementMenu leaveRequestManagementMenu = new LeaveRequestManagementMenu(leaveRequestService);
EmployeeMenu employeeMenu = new EmployeeMenu(userService, employeeService, attendanceRecordService, leaveRequestService, payrollService);
#endregion

User loggedInUser = new User();
bool isRunning = true;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Welcome to HR & Payroll managment system!");
Console.ResetColor();

do
{
    if (string.IsNullOrEmpty(userService.CurrentUser?.Email))
    {
        Console.WriteLine("[Menu]");
        Console.WriteLine("1. LogIn");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");

        Console.WriteLine("Choose an Option:");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                userService.CurrentUser = authService.Login();
                break;
            case "2":
                authService.SignUp();
                break;
            case "3":
                isRunning = false;
                break;
            default:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option");
                Console.ResetColor();
                break;
        }
    }else if (userService.CurrentUser.EmployeeProfile == null)
    {
        Console.Clear();
        Console.WriteLine("[Profile Setup]");
        Console.WriteLine("Enter Your First Name:");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Your Last Name:");
        string lastName = Console.ReadLine();

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Inputs Cant be empty!");
            Console.ResetColor();
            return;
        }
        
        var department = departmentService.GetAllDepartments().FirstOrDefault(d => d.DepartmentName == "Unassigned");
        var jobPosition = jobPositionService.GetAllJobPositions().FirstOrDefault(d => d.PositionTitle == "Employee");
        
        
        EmployeeProfile employeeProfile = new EmployeeProfile()
        {
            UserId = userService.CurrentUser.Id,
            FirstName = firstName,
            LastName = lastName,
            DepartmentId = department.Id,
            Department = department,
            JobPositionId = jobPosition.Id,
            JobPosition = jobPosition,
        };
        
        userService.CurrentUser.EmployeeProfile = employeeProfile;
        database.SaveChanges();
        
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Welcome {employeeProfile.FirstName} to HR & Payroll managment system!");
        Console.ResetColor();
    }
    else
    {
        if (userService.CurrentUser.Role.RoleName == "HR")
        {
            ShowHrMenu();
        }else if (userService.CurrentUser.Role.RoleName == "Employee")
        {
            employeeMenu.MainMenu();
        }
    }
} while (isRunning);

#region HR Menu
void ShowHrMenu()
{
    Console.Clear();
    bool isHrMenuRunning =  true;
    do
    {
        Console.WriteLine("=== HR Admin Dashboard ===");
        Console.WriteLine("1. Employee Management");
        Console.WriteLine("2. Department Management");
        Console.WriteLine("3. Job Position Management");
        Console.WriteLine("4. Attendance & Time Tracking");
        Console.WriteLine("5. Leave Request Management");
        Console.WriteLine("6. Payroll Management");
        Console.WriteLine("7. Reports & Analytics");
        Console.WriteLine("8. System Administration");
        Console.WriteLine("9. Back to Main Menu");
        Console.WriteLine("Choose an Option:");
        
        string choice =  Console.ReadLine();

        switch (choice)
        {
            case "1":
                employeeManagementMenu.MainMenu();
                break;
            case "2":
                departmentManagementMenu.MainMenu();
                break;
            case "3":
                jobPositionManagementMenu.MainMenu();
                break;
            case "4":
                attendanceTimeTrackingMenu.MainMenu();
                break;
            case "5":
                leaveRequestManagementMenu.MainMenu();
                break;
            case "9":
                loggedInUser = new User();
                isHrMenuRunning = false;
                
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
    } while (isHrMenuRunning);
}
#endregion