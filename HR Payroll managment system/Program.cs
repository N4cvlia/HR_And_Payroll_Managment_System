using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Validators;
using Microsoft.EntityFrameworkCore;

HRContext database =  new HRContext();
User loggedInUser = new User();
EmailSender  emailSender = new EmailSender();
Logging  logger = new Logging();
bool isRunning = true;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Welcome to HR & Payroll managment system!");
Console.ResetColor();

do
{
    if (string.IsNullOrEmpty(loggedInUser.Email))
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
                Login();
                break;
            case "2":
                SignUp();
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

// Authentication
#region Authentication
void SignUp()
{
    Console.Clear();
    Console.WriteLine("[SignUp Menu]");
    Console.Write("Enter Your Email: ");
    string email = Console.ReadLine();
    Console.Write("Enter Your Password: ");
    string password = Console.ReadLine();

    User user = new User()
    {
        Email = email,
        Password = password
    };
    
    LoginValidator loginValidator = new LoginValidator();
    
    var isLoginValid = loginValidator.Validate(user);

    if (isLoginValid.IsValid)
    {
        Console.Clear();
        bool isSuccessful = false;
        do
        {
            var roles = database.Roles.ToList();
            
            
            Console.WriteLine("[Choose Role]");
            foreach (var item in roles)
            {
                Console.WriteLine($"{item.Id}. {item.RoleName}");
            }

            Console.WriteLine("Choose an Option:");
            
            var choice1 = int.Parse(Console.ReadLine());
            var result = roles.FirstOrDefault(r => r.Id == choice1);

            if (result != null)
            {
                Console.Clear();
                
                Random random = new Random();
                int code = random.Next(1000, 9999);
                bool isVerified = false;

                EmailLog emailLog = new EmailLog()
                {
                    ToEmail = user.Email,
                    Subject = "Your Verification Code - HR Payroll System",
                    Body = $@"<div style='font-family: Arial, sans-serif; padding: 20px;'><h2>HR Payroll System - Verification Code</h2><p>Hello,</p><p>Your verification code is:</p><div style='font-size: 32px; font-weight: bold; color: #007cba; margin: 20px 0;'> {code}</div><p>Enter this code in the application to verify your email address.</p><p><strong>This code expires in 10 minutes.</strong></p><hr><p style='color: #666; font-size: 12px;'>If you didn't request this code, please ignore this email.</p></div>",
                    IsSent = false
                };
                
                emailSender.Send(emailLog);
                database.EmailLogs.Add(emailLog);
                database.SaveChanges();

                do
                {
                    Console.WriteLine("[Verify Email]");
                    Console.WriteLine("Enter Code Sent to your Email:");
                    string codeSent = Console.ReadLine();

                    if (codeSent == code.ToString())
                    {
                        User user1 = new User()
                        {
                            Email = user.Email,
                            Password = BCrypt.Net.BCrypt.HashPassword(password),
                            Role = result
                        };
                
                        database.Users.Add(user1);
                        database.SaveChanges();
                
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Signed Up!");
                        Console.ResetColor();
                        isSuccessful = true;
                        isVerified = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong Code Try again!");
                        Console.ResetColor();
                    }
                } while (!isVerified);
            }
            else
            {
               Console.Clear();
               Console.ForegroundColor = ConsoleColor.Red;
               Console.WriteLine("Invalid Option!");
               Console.ResetColor();
            }
        } while (!isSuccessful);
    }
    else
    {
        Console.Clear();
        foreach (var e in isLoginValid.Errors)
        {
           Console.ForegroundColor =  ConsoleColor.Red;
           Console.WriteLine(e);
           Console.ResetColor();
        }
    }
}

void Login()
{
    Console.Clear();
    Console.WriteLine("[Login Menu]");
    Console.Write("Enter Your Email: ");
    string email = Console.ReadLine();
    Console.Write("Enter Your Password: ");
    string password = Console.ReadLine();
    
    User user = database.Users.Include(u => u.Role).Include(u => u.EmployeeProfile).FirstOrDefault(u => u.Email == email);

    if (user == null)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Credentials");
        Console.ResetColor();
    }else if (BCrypt.Net.BCrypt.Verify(password, user.Password))
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Successfully Logged In!");
        Console.ResetColor();
        loggedInUser = user;

        ActivityLog activityLog = new ActivityLog()
        {
            UserId = user.Id,
            User = user,
            Action = "LogIn",
            Description = "User Logged into their account"
        };
        
        logger.LogActivity(activityLog);
        database.ActivityLogs.Add(activityLog);
        database.SaveChanges();
    }
}
#endregion

// HR Menu
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
                DepartmentManagement();
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
void DepartmentManagement()
{
    bool isRunning = true;
    Console.Clear();
    do
    {
        Console.WriteLine("=== Department Management ===");
        Console.WriteLine("1. View All Departments");
        Console.WriteLine("2. Add New Department");
        Console.WriteLine("3. Edit Department Details");
        Console.WriteLine("4. View Department Employees");
        Console.WriteLine("5. Department Salary Reports");
        Console.WriteLine("6. Back to HR Menu");
        Console.WriteLine("Choose an Option:");
      
        switch (Console.ReadLine())
        {
            case "1":
                ViewAllDepartments();
                break;
            case "2":
                AddNewDepartment();
                break;
            case "3":
                EditDepartmentDetails();
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

void ViewAllDepartments()
{
    Console.Clear();
    var allDepartments = database.Departments
        .Select(d => new { d.Id, d.DepartmentName, d.Employees.Count, d.Description }).ToList();
    Console.WriteLine("=== View All Departments ===");
    Console.WriteLine("─".PadRight(60, '─'));
    Console.WriteLine("ID  DEPARTMENT        EMPLOYEES    DESCRIPTION");
    Console.WriteLine("─".PadRight(60, '─'));
    
    foreach (var dept in allDepartments)
    {
        Console.WriteLine($"{dept.Id,-3} {dept.DepartmentName,-16}      {dept.Count,-10} {dept.Description}");
    }
    
    Console.WriteLine("─".PadRight(60, '─'));
    Console.ReadKey();
    Console.Clear();
}

void AddNewDepartment()
{
    Console.Clear();
    Console.WriteLine("=== Add New Department ===");
    Console.WriteLine("Enter Department Name:");
    string departmentName = Console.ReadLine();

    Console.WriteLine("Enter Department Description:");
    string departmentDescription = Console.ReadLine();
    
    Department newDepartment = new Department()
    {
        DepartmentName = departmentName,
        Description = departmentDescription,
    };

    DepartmentValidator departmentValidator = new DepartmentValidator();

    var isValidDepartment = departmentValidator.Validate(newDepartment);

    if (isValidDepartment.IsValid)
    {
        database.Departments.Add(newDepartment);
        database.SaveChanges();
        
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Successfully added new department!");
        Console.ResetColor();

        ActivityLog activityLog = new ActivityLog()
        {
            UserId = loggedInUser.Id,
            User = loggedInUser,
            Action = "Department Creation",
            Description = $"{newDepartment.DepartmentName} Was created"
        };
        logger.LogActivity(activityLog);
        database.ActivityLogs.Add(activityLog);
        database.SaveChanges();
    }
    else
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var e in isValidDepartment.Errors)
        {
            Console.WriteLine(e);
        }
        Console.ResetColor();
    }
}

void EditDepartmentDetails()
{
    Console.Clear();
    Console.WriteLine("=== Edit Department Details ===");
    Console.WriteLine("Enter Department Id you want to edit:");
    int departmentId = int.Parse(Console.ReadLine());
    
    var department = database.Departments.FirstOrDefault(d => d.Id == departmentId);

    if (department == null)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Department Not Found");
        Console.ResetColor();
    }else if (department.Id == 1)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Cannot Edit Unassigned Department");
        Console.ResetColor();
    }
    else
    {
        Console.Clear();
        Console.WriteLine("=== Edit Department Details ===");
        Console.WriteLine("Enter New Department Name:");
        string departmentName = Console.ReadLine();

        Console.WriteLine("Enter New Department Description:");
        string departmentDescription = Console.ReadLine();
    
        Department newDepartment = new Department()
        {
            DepartmentName = departmentName,
            Description = departmentDescription,
        };

        DepartmentValidator departmentValidator = new DepartmentValidator();

        var isValidDepartment = departmentValidator.Validate(newDepartment);

        if (isValidDepartment.IsValid)
        {
            department.DepartmentName = departmentName;
            department.Description = departmentDescription;
            database.SaveChanges();
        
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully edited the department!");
            Console.ResetColor();

            ActivityLog activityLog = new ActivityLog()
            {
                UserId = loggedInUser.Id,
                User = loggedInUser,
                Action = "Department Edited",
                Description = $"{newDepartment.DepartmentName} Was Edited"
            };
            logger.LogActivity(activityLog);
            database.ActivityLogs.Add(activityLog);
            database.SaveChanges();
        }
        else
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var e in isValidDepartment.Errors)
            {
                Console.WriteLine(e);
            }
            Console.ResetColor();
        }
    }
}

void ViewDepartmentEmployees()
{
    Console.Clear();
    Console.WriteLine("=== View Department Employees ===");
    Console.WriteLine("Enter Department Id To View Employees:");
    int departmentId = int.Parse(Console.ReadLine());
    
    var department = database.Departments.Include(d => d.Employees).FirstOrDefault(d => d.Id == departmentId);

    if (department == null)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Department Not Found");
        Console.ResetColor();
    }
    else
    {
        Console.Clear();
        
    }
}
#endregion