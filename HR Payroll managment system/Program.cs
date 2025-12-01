using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation;
using HR_Payroll_managment_system.Services;

// Services
#region Services
AuthService authService = new AuthService();
UserService userService = new UserService();
DepartmentService departmentService = new DepartmentService(userService);
JobPositionService jobPositionService = new JobPositionService(userService);
EmployeeService employeeService = new EmployeeService(userService);
AttendanceRecordService attendanceRecordService = new AttendanceRecordService(userService);
LeaveRequestService leaveRequestService = new LeaveRequestService(userService);
PayrollService payrollService = new PayrollService(userService);
BonusService bonusService = new BonusService(userService);
DeductionService deductionService = new DeductionService(userService);

EmployeeMenu employeeMenu = new EmployeeMenu(userService, employeeService, attendanceRecordService, leaveRequestService, payrollService);
HrMenu hrMenu = new HrMenu(userService, departmentService, jobPositionService, employeeService, attendanceRecordService, leaveRequestService, bonusService, deductionService, payrollService);
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


        EmployeeProfile employeeProfile = new EmployeeProfile();
        
        if (userService.CurrentUser.Role.RoleName == "HR")
        {
            var departmentHr = departmentService.GetAllDepartments().FirstOrDefault(d => d.DepartmentName == "HR Department");
            var jobPositionHr = jobPositionService.GetAllJobPositions()
                .FirstOrDefault(d => d.PositionTitle == "HR");
            
            employeeProfile.UserId = userService.CurrentUser.Id;
            employeeProfile.FirstName = firstName;
            employeeProfile.LastName = lastName;
            employeeProfile.DepartmentId = departmentHr.Id;
            employeeProfile.JobPositionId = jobPositionHr.Id;
        }
        else
        {
            var department = departmentService.GetAllDepartments().FirstOrDefault(d => d.DepartmentName == "Unassigned");
            var jobPosition = jobPositionService.GetAllJobPositions().FirstOrDefault(d => d.PositionTitle == "Employee");
            
            employeeProfile.UserId = userService.CurrentUser.Id;
            employeeProfile.FirstName = firstName;
            employeeProfile.LastName = lastName;
            employeeProfile.DepartmentId = department.Id;
            employeeProfile.JobPositionId = jobPosition.Id;
        }
        
        employeeService.AddEmployeeProfile(employeeProfile);
        
        userService.CurrentUser.EmployeeProfile = employeeService.GetByUserId(userService.CurrentUser.Id);
        
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Welcome {employeeProfile.FirstName} to HR & Payroll managment system!");
        Console.ResetColor();
    }
    else
    {
        if (userService.CurrentUser.Role.RoleName == "HR")
        {
            hrMenu.MainMenu();
        }else if (userService.CurrentUser.Role.RoleName == "Employee")
        {
            employeeMenu.MainMenu();
        }
    }
} while (isRunning);