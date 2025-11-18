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
                JobPositionManagement();
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

// Department Management
#region DepartmentManagement
// void DepartmentManagement()
// {
//     bool isRunning = true;
//     Console.Clear();
//     do
//     {
//         Console.WriteLine("=== Department Management ===");
//         Console.WriteLine("1. View All Departments");
//         Console.WriteLine("2. Add New Department");
//         Console.WriteLine("3. Edit Department Details");
//         Console.WriteLine("4. View Department Employees");
//         Console.WriteLine("5. Department Salary Reports");
//         Console.WriteLine("6. Back to HR Menu");
//         Console.WriteLine("Choose an Option:");
//       
//         switch (Console.ReadLine())
//         {
//             case "1":
//                 ViewAllDepartments();
//                 break;
//             case "2":
//                 AddNewDepartment();
//                 break;
//             case "3":
//                 EditDepartmentDetails();
//                 break;
//             case "4":
//                 ViewDepartmentEmployees();
//                 break;
//             case "5":
//                 DepartmentSalaryReports();
//                 break;
//             case "6":
//                 Console.Clear();
//                 isRunning = false;
//                 break;
//             default:
//                 Console.Clear();
//                 Console.ForegroundColor = ConsoleColor.Red;
//                 Console.WriteLine("Invalid Input!");
//                 Console.ResetColor();
//                 break;
//         }
//     } while (isRunning);
// }

// void ViewAllDepartments()
// {
//     Console.Clear();
//     var allDepartments = database.Departments
//         .Select(d => new { d.Id, d.DepartmentName, d.Employees.Count, d.Description }).ToList();
//     
//     int maxDeptNameLength = allDepartments.Max(d => d.DepartmentName.Length);
//     int deptColumnWidth = Math.Max(maxDeptNameLength, 10) + 2;
//
//     int maxDescLength = allDepartments.Max(d => d.Description?.Length ?? 0);
//     int descColumnWidth = Math.Max(maxDescLength, 11) + 2;
//
//     int totalWidth = deptColumnWidth + descColumnWidth + 20;
//
//     Console.WriteLine("=== View All Departments ===");
//     Console.WriteLine("─".PadRight(totalWidth, '─'));
//     Console.WriteLine($"ID  {"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  {"DESCRIPTION".PadRight(descColumnWidth)}");
//     Console.WriteLine("─".PadRight(totalWidth, '─'));
//
//     foreach (var dept in allDepartments)
//     {
//         Console.WriteLine($"{dept.Id,-3} {dept.DepartmentName.PadRight(deptColumnWidth)} {dept.Count,-10} {dept.Description?.PadRight(descColumnWidth)}");
//     }
//
//     Console.WriteLine("─".PadRight(totalWidth, '─'));
//     Console.WriteLine("Click Any Key To Exit.");
//     Console.ReadKey();
//     Console.Clear();
// }

// void AddNewDepartment()
// {
//     Console.Clear();
//     Console.WriteLine("=== Add New Department ===");
//     Console.WriteLine("Enter Department Name:");
//     string departmentName = Console.ReadLine();
//
//     Console.WriteLine("Enter Department Description:");
//     string departmentDescription = Console.ReadLine();
//     
//     Department newDepartment = new Department()
//     {
//         DepartmentName = departmentName,
//         Description = departmentDescription,
//     };
//
//     DepartmentValidator departmentValidator = new DepartmentValidator();
//
//     var isValidDepartment = departmentValidator.Validate(newDepartment);
//
//     if (isValidDepartment.IsValid)
//     {
//         database.Departments.Add(newDepartment);
//         database.SaveChanges();
//         
//         Console.Clear();
//         Console.ForegroundColor = ConsoleColor.Green;
//         Console.WriteLine("Successfully added new department!");
//         Console.ResetColor();
//
//         ActivityLog activityLog = new ActivityLog()
//         {
//             UserId = loggedInUser.Id,
//             Action = "Department Creation",
//             Description = $"{newDepartment.DepartmentName} Was created"
//         };
//         logger.LogActivity(activityLog, loggedInUser.Email);
//         database.ActivityLogs.Add(activityLog);
//         database.SaveChanges();
//     }
//     else
//     {
//         Console.Clear();
//
//         Console.ForegroundColor = ConsoleColor.Red;
//         foreach (var e in isValidDepartment.Errors)
//         {
//             Console.WriteLine(e);
//         }
//         Console.ResetColor();
//     }
// }

// void EditDepartmentDetails()
// {
//     Console.Clear();
//     Console.WriteLine("=== Edit Department Details ===");
//     Console.WriteLine("Enter Department Id you want to edit:");
//     int departmentId = int.Parse(Console.ReadLine());
//     
//     var department = database.Departments.FirstOrDefault(d => d.Id == departmentId);
//
//     if (department == null)
//     {
//         Console.Clear();
//         Console.ForegroundColor = ConsoleColor.Red;
//         Console.WriteLine("Department Not Found");
//         Console.ResetColor();
//     }else if (department.Id == 1)
//     {
//         Console.Clear();
//         Console.ForegroundColor = ConsoleColor.Red;
//         Console.WriteLine("Cannot Edit Unassigned Department");
//         Console.ResetColor();
//     }
//     else
//     {
//         Console.Clear();
//         Console.WriteLine("=== Edit Department Details ===");
//         Console.WriteLine("Enter New Department Name:");
//         string departmentName = Console.ReadLine();
//
//         Console.WriteLine("Enter New Department Description:");
//         string departmentDescription = Console.ReadLine();
//     
//         Department newDepartment = new Department()
//         {
//             DepartmentName = departmentName,
//             Description = departmentDescription,
//         };
//
//         DepartmentValidator departmentValidator = new DepartmentValidator();
//
//         var isValidDepartment = departmentValidator.Validate(newDepartment);
//
//         if (isValidDepartment.IsValid)
//         {
//             department.DepartmentName = departmentName;
//             department.Description = departmentDescription;
//             database.SaveChanges();
//         
//             Console.Clear();
//             Console.ForegroundColor = ConsoleColor.Green;
//             Console.WriteLine("Successfully edited the department!");
//             Console.ResetColor();
//
//             ActivityLog activityLog = new ActivityLog()
//             {
//                 UserId = loggedInUser.Id,
//                 Action = "Department Edited",
//                 Description = $"{newDepartment.DepartmentName} Was Edited"
//             };
//             logger.LogActivity(activityLog, loggedInUser.Email);
//             database.ActivityLogs.Add(activityLog);
//             database.SaveChanges();
//         }
//         else
//         {
//             Console.Clear();
//
//             Console.ForegroundColor = ConsoleColor.Red;
//             foreach (var e in isValidDepartment.Errors)
//             {
//                 Console.WriteLine(e);
//             }
//             Console.ResetColor();
//         }
//     }
// }

// void ViewDepartmentEmployees()
// {
//     Console.Clear();
//     Console.WriteLine("=== View Department Employees ===");
//     Console.WriteLine("Enter Department Id To View Employees:");
//     int departmentId = int.Parse(Console.ReadLine());
//     
//     var department = database.Departments
//         .Include(d => d.Employees)
//         .ThenInclude(d => d.JobPosition)
//         .Include(d => d.Employees)
//         .ThenInclude(d => d.User)
//         .FirstOrDefault(d => d.Id == departmentId);
//
//     if (department == null)
//     {
//         Console.Clear();
//         Console.ForegroundColor = ConsoleColor.Red;
//         Console.WriteLine("Department Not Found");
//         Console.ResetColor();
//     }
//     else
//     {
//         Console.Clear();
//         Console.WriteLine($"DEPARTMENT: {department.DepartmentName}");
//         Console.WriteLine($"EMPLOYEES: {department.Employees.Count(e => e.IsActive)}");
//         Console.WriteLine("─".PadRight(70, '─'));
//     
//         if (!department.Employees.Any(e => e.IsActive))
//         {
//             Console.WriteLine("No employees in this department");
//             Console.WriteLine("─".PadRight(70, '─'));
//             Console.WriteLine("Click Any Key To Exit.");
//             Console.ReadKey();
//             Console.Clear();
//             return;
//         }
//
//         Console.WriteLine("ID  NAME                      POSITION           EMAIL");
//         Console.WriteLine("─".PadRight(70, '─'));
//     
//         foreach (var emp in department.Employees.Where(e => e.IsActive))
//         {
//             Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.JobPosition?.PositionTitle,-17} {emp.User.Email}");
//         }
//         Console.WriteLine("─".PadRight(70, '─'));
//         Console.WriteLine("Click Any Key To Exit.");
//         Console.ReadKey();
//         Console.Clear();
//     }
// }
//
// void DepartmentSalaryReports()
// {
//     Console.Clear();
//     var report = database.Departments
//         .Select(d => new 
//         {
//             DepartmentName = d.DepartmentName,
//             EmployeeCount = d.Employees.Count(e => e.IsActive),
//             TotalSalary = d.Employees.Where(e => e.IsActive).Sum(e => (decimal?)e.BaseSalary) ?? 0,
//             AverageSalary = d.Employees.Where(e => e.IsActive).Average(e => (decimal?)e.BaseSalary) ?? 0
//         })
//         .ToList();
//     
//     int maxDeptNameLength = report.Max(d => d.DepartmentName.Length);
//     int deptColumnWidth = Math.Max(maxDeptNameLength, 10) + 2;
//
//     Console.WriteLine("=== DEPARTMENT SALARY REPORT ===");
//     Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));
//     Console.WriteLine($"{"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  AVG SALARY  TOTAL SALARY");
//     Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));
//
//     foreach (var dept in report)
//     {
//         Console.WriteLine($"{dept.DepartmentName.PadRight(deptColumnWidth)} {dept.EmployeeCount,-10} ${dept.AverageSalary,-10:N0} ${dept.TotalSalary:N0}");
//     }
//
//     var companyTotal = report.Sum(r => r.TotalSalary);
//     var companyAvg = report.Average(r => r.AverageSalary);
//     var totalEmployees = report.Sum(r => r.EmployeeCount);
//
//     Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));
//     Console.WriteLine($"{"TOTALS:".PadRight(deptColumnWidth)} {totalEmployees,-10} ${companyAvg,-10:N0} ${companyTotal:N0}");
//     Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));
//     Console.WriteLine("Click Any Key To Exit.");
//     Console.ReadKey();
//     Console.Clear();
// }
#endregion

// Job Position Management
#region JobPositionManagement

void JobPositionManagement()
{
    bool isRunning = true;
    Console.Clear();
    do
    {
        Console.WriteLine("=== Job Position Management ===");
        Console.WriteLine("1. View All Positions");
        Console.WriteLine("2. Create New Positions");
        Console.WriteLine("3. Edit Position Details");
        Console.WriteLine("4. Assign Salary Ranges");
        Console.WriteLine("5. Back to HR Menu");
        Console.WriteLine("Choose an Option:");

        switch (Console.ReadLine())
        {
            case "1":
                ViewAllPositions();
                break;
            case "2":
                CreateNewPositions();
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

void ViewAllPositions()
{
    Console.Clear();
    var allPositions = database.JobPositions
        .Include(jp => jp.Department)
        .Select(jp => new { 
            jp.Id, 
            jp.PositionTitle, 
            Department = jp.Department.DepartmentName,
            jp.MinSalary, 
            jp.MaxSalary 
        }).ToList();
    
    int maxTitleLength = allPositions.Max(p => p.PositionTitle.Length);
    int titleColumnWidth = Math.Max(maxTitleLength, 8) + 2;

    int maxDeptLength = allPositions.Max(p => p.Department.Length);
    int deptColumnWidth = Math.Max(maxDeptLength, 10) + 2;

    int totalWidth = titleColumnWidth + deptColumnWidth + 25;

    Console.WriteLine("=== VIEW ALL POSITIONS ===");
    Console.WriteLine("─".PadRight(totalWidth, '─'));
    Console.WriteLine($"ID  {"POSITION".PadRight(titleColumnWidth)} {"DEPARTMENT".PadRight(deptColumnWidth)} SALARY RANGE");
    Console.WriteLine("─".PadRight(totalWidth, '─'));

    foreach (var pos in allPositions)
    {
        string salaryRange = $"${pos.MinSalary:N0}-${pos.MaxSalary:N0}";
        Console.WriteLine($"{pos.Id,-3} {pos.PositionTitle.PadRight(titleColumnWidth)} {pos.Department.PadRight(deptColumnWidth)} {salaryRange}");
    }

    Console.WriteLine("─".PadRight(totalWidth, '─'));
    Console.WriteLine($"Total: {allPositions.Count} positions");
    Console.WriteLine("Click Any Key To Exit.");
    Console.ReadKey();
    Console.Clear();
}

void CreateNewPositions()
{
    var departments = database.Departments.Select(d => new { d.Id, d.DepartmentName, d.Employees.Count, d.Description }).ToList();

    if (departments.Count() < 2) return;
    
    Console.Clear();
    Console.WriteLine("=== Create New Positions ===");
    Console.WriteLine("Enter Position Title:");
    string positionTitle = Console.ReadLine();
    Console.WriteLine("Enter Position Description:");
    string positionDescription = Console.ReadLine();
    Console.WriteLine("Enter Min Salary:");
    decimal minSalary = decimal.Parse(Console.ReadLine());
    Console.WriteLine("Enter Max Salary:");
    decimal maxSalary = decimal.Parse(Console.ReadLine());

    JobPositionValidator jobPositionValidator = new JobPositionValidator();

    JobPosition newJobPosition = new JobPosition()
    {
        PositionTitle = positionTitle,
        Description = positionDescription,
        MinSalary = minSalary,
        MaxSalary = maxSalary,
    };

    var isPositionValid = jobPositionValidator.Validate(newJobPosition);

    if (isPositionValid.IsValid)
    {
        bool isRunning2 = true;
        do
        {
            Console.Clear();
            int maxDeptNameLength = departments.Max(d => d.DepartmentName.Length);
            int deptColumnWidth = Math.Max(maxDeptNameLength, 10) + 2;

            int maxDescLength = departments.Max(d => d.Description?.Length ?? 0);
            int descColumnWidth = Math.Max(maxDescLength, 11) + 2;

            int totalWidth = deptColumnWidth + descColumnWidth + 20;

            Console.WriteLine("=== Create New Positions ===");
            Console.WriteLine("─".PadRight(totalWidth, '─'));
            Console.WriteLine($"ID  {"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  {"DESCRIPTION".PadRight(descColumnWidth)}");
            Console.WriteLine("─".PadRight(totalWidth, '─'));

            foreach (var dept in departments)
            {
                Console.WriteLine($"{dept.Id,-3} {dept.DepartmentName.PadRight(deptColumnWidth)} {dept.Count,-10} {dept.Description?.PadRight(descColumnWidth)}");
            }

            Console.WriteLine("─".PadRight(totalWidth, '─'));
            Console.WriteLine("Choose a Department Id: ");
            int departmentId = int.Parse(Console.ReadLine());

            var department = departments.FirstOrDefault(d => d.Id == departmentId);

            if (department == null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input!");
                Console.ResetColor();
            }else if (department.DepartmentName == "Employee")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Job Position Cannot Be Assigned To Unassigned Department!");
                Console.ResetColor();
            }
            else
            {
                ActivityLog activityLog = new ActivityLog()
                {
                    UserId = loggedInUser.Id,
                    Action = "Job Position Creation",
                    Description = $"{department.DepartmentName} Was Created"
                };
                
                logger.LogActivity(activityLog, loggedInUser.Email);
                database.ActivityLogs.Add(activityLog);
                newJobPosition.DepartmentId = departmentId;
                database.JobPositions.Add(newJobPosition);
                database.SaveChanges();
                
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Created a New Job Position!");
                Console.ResetColor();
                isRunning2 = false;
            }
        } while (isRunning2);
    }
    else
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var e in isPositionValid.Errors)
        {
            Console.WriteLine(e);
        }
        Console.ResetColor();
    }
}
#endregion