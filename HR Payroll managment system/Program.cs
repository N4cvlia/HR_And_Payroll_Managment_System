using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation;
using HR_Payroll_managment_system.Services;
using HR_Payroll_managment_system.Validators;
using Microsoft.EntityFrameworkCore;

HRContext database =  new HRContext();
AuthService authService = new AuthService();
UserService userService = new UserService();

DepartmentService departmentService = new DepartmentService(userService);
DepartmentManagementMenu departmentManagementMenu = new DepartmentManagementMenu(departmentService);

JobPositionService jobPositionService = new JobPositionService(userService);
JobPositionManagementMenu jobPositionManagementMenu = new JobPositionManagementMenu(departmentService, jobPositionService);

User loggedInUser = new User();
bool isRunning = true;

EmailSender  emailSender = new EmailSender();
Logging  logger = new Logging();


Console.Clear();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Welcome to HR & Payroll managment system!");
Console.ResetColor();

do
{
    if (string.IsNullOrEmpty(loggedInUser?.Email))
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
                loggedInUser = authService.Login();
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
    }else if (loggedInUser.EmployeeProfile == null)
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
        
        var department = database.Departments.FirstOrDefault(d => d.DepartmentName == "Unassigned");
        var jobPosition = database.JobPositions.FirstOrDefault(d => d.PositionTitle == "Employee");
        
        
        EmployeeProfile employeeProfile = new EmployeeProfile()
        {
            UserId = loggedInUser.Id,
            FirstName = firstName,
            LastName = lastName,
            DepartmentId = department.Id,
            Department = department,
            JobPositionId = jobPosition.Id,
            JobPosition = jobPosition,
        };
        
        loggedInUser.EmployeeProfile = employeeProfile;
        database.SaveChanges();
        
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Welcome {employeeProfile.FirstName} to HR & Payroll managment system!");
        Console.ResetColor();
    }
    else
    {
        userService.CurrentUser = loggedInUser;
        
        if (loggedInUser.Role.RoleName == "HR")
        {
            ShowHrMenu();
        }else if (loggedInUser.Role.RoleName == "Employee")
        {
            Console.WriteLine("[Employee Menu]");
            Console.ReadKey();
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
                EmployeeManagement();
                break;
            case "2":
                departmentManagementMenu.MainMenu();
                break;
            case "3":
                jobPositionManagementMenu.MainMenu();
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

// Employee Menu
#region Employee Menu
void ShowEmployeeMenu()
{
    Console.Clear();
    bool isEmployeeMenuRunning =  true;
    do
    {
        Console.WriteLine("=== Employee Dashboard ===");
        Console.WriteLine("9. Back to Main Menu");
        Console.WriteLine("Choose an Option:");
        
        string choice =  Console.ReadLine();

        switch (choice)
        {
            case "9":
                loggedInUser = new User();
                isEmployeeMenuRunning = false;
                
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
#endregion

// Employee Management
#region EmployeeManagement
void EmployeeManagement()
{
    bool isRunning = true;
    Console.Clear();
    do
    {
      Console.WriteLine("=== Employee Management ===");
      Console.WriteLine("1. View All Employees");
      Console.WriteLine("2. Add New Employee");
      Console.WriteLine("3. Edit Employee Profile");
      Console.WriteLine("4. Assign Department/Position");
      Console.WriteLine("5. Deactivate/Reactivate Employee");
      Console.WriteLine("6. View Employee Details");
      Console.WriteLine("7. Search Employees");
      Console.WriteLine("8. Back to HR Menu");
      Console.WriteLine("Choose an Option:");
      
      switch (Console.ReadLine())
      {
          case "1":
              ViewAllEmployees();
              break;
          case "8":
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

void ViewAllEmployees()
{
    var allEmployees = database.Employees.Select(e => new {e.Id, e.FirstName, e.LastName, e.Department.DepartmentName, e.JobPosition.PositionTitle, e.User.Email}).ToList();
    
    Console.Clear();
    
    Console.WriteLine("=== View All Employees ===");
    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
    foreach (var emp in allEmployees)
    {
        Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.PositionTitle,-17} {emp.Email,-18}");
    }
    
    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    Console.WriteLine("Click any key to exit.");
    Console.ReadKey();
    
    Console.Clear();
}
#endregion