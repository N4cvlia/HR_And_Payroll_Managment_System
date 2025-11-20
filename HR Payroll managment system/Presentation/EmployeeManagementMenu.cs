using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class EmployeeManagementMenu
{
    EmployeeService _employeeService;
    DepartmentService _departmentService;
    JobPositionService _jobPositionService;

    public EmployeeManagementMenu(EmployeeService employeeService,  DepartmentService departmentService,  JobPositionService jobPositionService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _jobPositionService = jobPositionService;
    }
    
    public void MainMenu()
    {
        bool isRunning = true;
        Console.Clear();
        do
        {
            Console.WriteLine("=== Employee Management ===");
            Console.WriteLine("1. View All Employees");
            Console.WriteLine("2. Edit Employee Profile");
            Console.WriteLine("3. Assign Department/Position");
            Console.WriteLine("4. Deactivate/Reactivate Employee");
            Console.WriteLine("5. View Employee Details");
            Console.WriteLine("6. Search Employees");
            Console.WriteLine("7. Back to HR Menu");
            Console.WriteLine("Choose an Option:");
      
            switch (Console.ReadLine())
            {
                case "1":
                    ViewAllMenu();
                    break;
                case "2":
                    EditMenu();
                    break;
                case "3":
                    AssignMenu();
                    break;
                case "4":
                    IsActiveMenu();
                    break;
                case "5":
                    ViewEmployeeMenu();
                    break;
                case "6":
                    SearchMenu();
                    break;
                case "7":
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
    
    // Menu Fucntions
    #region Menu Functions
    public void ViewAllMenu()
    {
        var allEmployees = _employeeService.GetAllEmployeesWithDetails();
    
        Console.Clear();
    
        Console.WriteLine("=== View All Employees ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
        foreach (var emp in allEmployees)
        {
            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
        }
    
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Click any key to exit.");
        Console.ReadKey();
    
        Console.Clear();
    }

    public void EditMenu()
    {
        Console.Clear();
    
        var allEmployees = _employeeService.GetAllEmployeesWithDetails();
        Console.WriteLine("=== Edit Employee Profile ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
        foreach (var emp in allEmployees)
        {
            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
        }
    
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Enter an employee id:");
        int employeeId = int.Parse(Console.ReadLine());
        
        var employee = _employeeService.GetEmployeeById(employeeId);

        if (employee == null)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Employee not found!");
            Console.ResetColor();
        }
        else
        {
            EditMenuOptionsMenu(employee);
        }
    }

    public void EditMenuOptionsMenu(EmployeeProfile  employee)
    {
        bool isRunning = true;

        do
        {
            Console.Clear();
            Console.WriteLine("=== Edit Employee Profile ===");
            Console.WriteLine("1. Edit Employee First Name");
            Console.WriteLine("2. Edit Employee Last Name");
            Console.WriteLine("3. Edit Employee Base Salary");
            Console.WriteLine("4. Back to HR Menu");
            Console.WriteLine("Choose an option:");
                
            string choice =  Console.ReadLine();

            switch (choice)
            {
                case "1":
                        Console.Clear();
                        Console.WriteLine("=== Edit Employee Profile ===");
                        Console.WriteLine("Enter New Employee First Name:");
                        string newFirstName = Console.ReadLine();
                        
                        employee.FirstName = newFirstName;
                        var result = _employeeService.UpdateEmployee(employee);
                        
                        if (result.Success)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successfully Edited Employee Profile!");
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
                        isRunning = false;
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("=== Edit Employee Profile ===");
                    Console.WriteLine("Enter New Employee Last Name:");
                    string newLastName = Console.ReadLine();
                        
                    employee.LastName = newLastName;
                    var result2 = _employeeService.UpdateEmployee(employee);
                        
                    if (result2.Success)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Edited Employee Profile!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (var e in result2.Errors)
                        {
                            Console.WriteLine(e);
                        }
                        Console.ResetColor();
                    }
                    isRunning = false;
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("=== Edit Employee Profile ===");
                    Console.WriteLine("Enter New Employee Base Salary:");
                    decimal newBaseSalary = decimal.Parse(Console.ReadLine());
                        
                    employee.BaseSalary = newBaseSalary;
                    var result3 = _employeeService.UpdateEmployee(employee);
                        
                    if (result3.Success)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Edited Employee Profile!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (var e in result3.Errors)
                        {
                            Console.WriteLine(e);
                        }
                        Console.ResetColor();
                    }
                    isRunning = false;
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

    public void AssignMenu()
    {
        Console.Clear();
    
        var allEmployees = _employeeService.GetAllEmployeesWithDetails();
        Console.WriteLine("=== Edit Employee Profile ===");
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
        
        var employee = _employeeService.GetEmployeeById(employeeId);

        if (employee != null)
        {
            Console.Clear();
            var allDepartments = _departmentService.GetAllDepartments()
                .Select(d => new
                {
                    d.Id,
                    d.DepartmentName,
                    Count = d.Employees != null ? d.Employees.Count(e => e.IsActive) : 0,
                    d.Description
                });
    
            int maxDeptNameLength = allDepartments.Max(d => d.DepartmentName.Length);
            int deptColumnWidth = Math.Max(maxDeptNameLength, 10) + 2;

            int maxDescLength = allDepartments.Max(d => d.Description?.Length ?? 0);
            int descColumnWidth = Math.Max(maxDescLength, 11) + 2;

            int totalWidth = deptColumnWidth + descColumnWidth + 20;

            Console.WriteLine("=== Edit Employee Profile ===");
            Console.WriteLine("─".PadRight(totalWidth, '─'));
            Console.WriteLine($"ID  {"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  {"DESCRIPTION".PadRight(descColumnWidth)}");
            Console.WriteLine("─".PadRight(totalWidth, '─'));

            foreach (var dept in allDepartments)
            {
                Console.WriteLine($"{dept.Id,-3} {dept.DepartmentName.PadRight(deptColumnWidth)} {dept.Count,-10} {dept.Description?.PadRight(descColumnWidth)}");
            }

            Console.WriteLine("─".PadRight(totalWidth, '─'));
            Console.WriteLine("Choose Department Id:");

            if (!int.TryParse(Console.ReadLine(), out int departmentId))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input!");
                Console.ResetColor();
                return;
            }

            var result = _departmentService.GetDepartmentByIdWithJobPosition(departmentId);

            if(result == null)
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No Departments Found!");
                Console.ResetColor();
            }else if (result.JobPositions.Count() == 0)
            {
                Console.Clear();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{result.DepartmentName} has no available job positions!");
                Console.ResetColor();
            }else if (result != null)
            {
                Console.Clear();
                var allPositions = result.JobPositions
                    .Select(jp => new { 
                        jp.Id, 
                        jp.PositionTitle, 
                        Department = jp.Department.DepartmentName,
                        jp.MinSalary, 
                        jp.MaxSalary 
                    })
                    .ToList();
    
                int maxTitleLength = allPositions.Max(p => p.PositionTitle.Length);
                int titleColumnWidth = Math.Max(maxTitleLength, 8) + 2;

                int maxDeptLength = allPositions.Max(p => p.Department.Length);
                int deptColumnWidth2 = Math.Max(maxDeptLength, 10) + 2;

                int totalWidth2 = titleColumnWidth + deptColumnWidth + 25;

                Console.WriteLine("=== VIEW ALL POSITIONS ===");
                Console.WriteLine("─".PadRight(totalWidth2, '─'));
                Console.WriteLine($"ID  {"POSITION".PadRight(titleColumnWidth)} {"DEPARTMENT".PadRight(deptColumnWidth2)} SALARY RANGE");
                Console.WriteLine("─".PadRight(totalWidth2, '─'));

                foreach (var pos in allPositions)
                {
                    string salaryRange = $"${pos.MinSalary:N0}-${pos.MaxSalary:N0}";
                    Console.WriteLine($"{pos.Id,-3} {pos.PositionTitle.PadRight(titleColumnWidth)} {pos.Department.PadRight(deptColumnWidth2)} {salaryRange}");
                }

                Console.WriteLine("─".PadRight(totalWidth2, '─'));
                Console.WriteLine("Choose Job Position Id:");

                if (!int.TryParse(Console.ReadLine(), out int positionId))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input!");
                    Console.ResetColor();
                    return;
                }
                
                var result2 = _jobPositionService.GetJobPositionById(positionId);

                if (result2 != null)
                {
                    employee.DepartmentId = departmentId;
                    employee.JobPositionId = positionId;

                    var updateResult = _employeeService.AssignDepartmentAndPosition(employee);

                    if (updateResult)
                    {
                        Console.Clear();
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Succesfully Assigned Department And Position!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();
                        
                        Console.ForegroundColor =  ConsoleColor.Red;
                        Console.WriteLine("Something went wrong!");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Position Found!");
                    Console.ResetColor();
                }
            }
        }
        else
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Employees Found!");
            Console.ResetColor();
        }
    }

    public void IsActiveMenu()
    {
        Console.Clear();
        bool isRunning = true;

        do
        {
            Console.WriteLine("=== Deactivate/Reactivate Employee ===");
            Console.WriteLine("1. Deactivate Employee");
            Console.WriteLine("2. Reactivate Employee");
            Console.WriteLine("3. Back To HR Menu");
            Console.WriteLine("Choose an Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    var employees = _employeeService.GetAllEmployeesWithDetails().Where(e => e.IsActive).ToList();
                    Console.Clear();
                    Console.WriteLine("=== Deactivate Employee ===");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    
                    if (!employees.Any(e => e.IsActive))
                    {
                        Console.WriteLine("No employees active");
                        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                        Console.WriteLine("Click Any Key To Exit.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
    
                    foreach (var emp in employees)
                    {
                        Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
                    }
    
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Enter Employee Id:");
                    if (!int.TryParse(Console.ReadLine(), out int employeeId))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input!");
                        Console.ResetColor();
                        break;
                    }
        
                    var employee = _employeeService.GetEmployeeById(employeeId);

                    if (employee != null)
                    {
                        employee.IsActive = false;
                        var result = _employeeService.UpdateEmployee(employee);

                        if (result.Success)
                        {
                            Console.Clear();
                            
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Employee has been Deactivated!");
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
                
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No Employee Found!");
                        Console.ResetColor();
                    }
                    break;
                case "2":
                    var employees2 = _employeeService.GetAllEmployeesWithDetails().Where(e => !e.IsActive).ToList();
                    Console.Clear();
                    Console.WriteLine("=== Activate Employee ===");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    
                    if (employees2.Any(e => e.IsActive))
                    {
                        Console.WriteLine("No employees Deactivated");
                        Console.WriteLine("─".PadRight(70, '─'));
                        Console.WriteLine("Click Any Key To Exit.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
    
                    foreach (var emp in employees2)
                    {
                        Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
                    }
    
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Enter Employee Id:");
                    if (!int.TryParse(Console.ReadLine(), out int employeeId2))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input!");
                        Console.ResetColor();
                        break;
                    }
        
                    var employee2 = _employeeService.GetEmployeeById(employeeId2);

                    if (employee2 != null)
                    {
                        employee2.IsActive = true;
                        var result = _employeeService.UpdateEmployee(employee2);

                        if (result.Success)
                        {
                            Console.Clear();
                            
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Employee has been Activated!");
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
                
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No Employee Found!");
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

    public void ViewEmployeeMenu()
    {
        var employees = _employeeService.GetAllEmployeesWithDetails().ToList();
        Console.Clear();
        Console.WriteLine("=== View Employee Details ===");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
    
        foreach (var emp in employees)
        {
            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
        }
    
        Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
        Console.WriteLine("Enter Employee Id:");

        if (!int.TryParse(Console.ReadLine(), out int employeeId))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }
        
        var  employee = employees.FirstOrDefault(e => e.Id == employeeId);

        if (employee != null)
        {
            Console.Clear();
            Console.WriteLine("=== View Employee Details ===");
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
        else
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Employee Found!");
            Console.ResetColor();
        }
    }

    public void SearchMenu()
    {
        Console.Clear();
        bool isRunning = true;
        do
        {
            Console.WriteLine("=== Search Employees ===");
            Console.WriteLine("1. Search With Employee Name");
            Console.WriteLine("2. Search With Department");
            Console.WriteLine("3. Search With Job Position");
            Console.WriteLine("4. Back To HR Menu");
            Console.WriteLine("Choose An Option:");

            switch (Console.ReadLine())
            {
                case"1":
                    Console.Clear();
                    Console.WriteLine("=== Search Employees ===");
                    Console.WriteLine("Enter Employee Name:");
                    
                    string employeeName = Console.ReadLine();

                    var result = _employeeService.GetAllEmployeesWithDetails().Where(e => e.FirstName.ToLower() == employeeName.Trim().ToLower()).ToList();
                    
                    Console.Clear();
                    Console.WriteLine("=== Search Employees ===");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("ID  NAME                       DEPT          POSITION           EMAIL");
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    
                    if (!result.Any(e => e.IsActive))
                    {
                        Console.WriteLine("No employees with this Name");
                        Console.WriteLine("─".PadRight(70, '─'));
                        Console.WriteLine("Click Any Key To Exit.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
    
                    foreach (var emp in result)
                    {
                        Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.DepartmentName,-9}    {emp.JobPositionName,-17} {emp.Email,-18}");
                    }
    
                    Console.WriteLine("───────────────────────────────────────────────────────────────────────────────");
                    Console.WriteLine("Press  any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case"2":
                    Console.Clear();
                    var allDepartments = _departmentService.GetAllDepartments()
                        .Select(d => new
                        {
                            d.Id,
                            d.DepartmentName,
                            Count = d.Employees != null ? d.Employees.Count(e => e.IsActive) : 0,
                            d.Description
                        });
    
                    int maxDeptNameLength = allDepartments.Max(d => d.DepartmentName.Length);
                    int deptColumnWidth = Math.Max(maxDeptNameLength, 10) + 2;

                    int maxDescLength = allDepartments.Max(d => d.Description?.Length ?? 0);
                    int descColumnWidth = Math.Max(maxDescLength, 11) + 2;

                    int totalWidth = deptColumnWidth + descColumnWidth + 20;

                    Console.WriteLine("=== Search Employees ===");
                    Console.WriteLine("─".PadRight(totalWidth, '─'));
                    Console.WriteLine($"ID  {"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  {"DESCRIPTION".PadRight(descColumnWidth)}");
                    Console.WriteLine("─".PadRight(totalWidth, '─'));

                    foreach (var dept in allDepartments)
                    {
                        Console.WriteLine($"{dept.Id,-3} {dept.DepartmentName.PadRight(deptColumnWidth)} {dept.Count,-10} {dept.Description?.PadRight(descColumnWidth)}");
                    }

                    Console.WriteLine("─".PadRight(totalWidth, '─'));
                    Console.WriteLine("Choose Department Id:");
                    if (!int.TryParse(Console.ReadLine(), out int departmentId))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input!");
                        Console.ResetColor();
                        break;
                    }
    
                    var department = _departmentService.GetDepartmentByIdWithDetails(departmentId);

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
                        Console.WriteLine("=== Search Employees ===");
                        Console.WriteLine($"DEPARTMENT: {department.DepartmentName}");
                        Console.WriteLine($"EMPLOYEES: {department.Employees.Count(e => e.IsActive)}");
                        Console.WriteLine("─".PadRight(70, '─'));
    
                        if (!department.Employees.Any(e => e.IsActive))
                        {
                            Console.WriteLine("No employees in this department");
                            Console.WriteLine("─".PadRight(70, '─'));
                            Console.WriteLine("Click Any Key To Exit.");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }

                        Console.WriteLine("ID  NAME                      POSITION           EMAIL");
                        Console.WriteLine("─".PadRight(70, '─'));
    
                        foreach (var emp in department.Employees.Where(e => e.IsActive))
                        {
                            Console.WriteLine($"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.JobPosition?.PositionTitle,-17} {emp.User.Email}");
                        }
                        Console.WriteLine("─".PadRight(70, '─'));
                        Console.WriteLine("Click Any Key To Exit.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case "3":
                    Console.Clear();
                    var jobPositions = _jobPositionService.GetAllJobPositions();

                    Console.WriteLine("=== Search Employees ===");
                    Console.WriteLine("ID  POSITION           DEPARTMENT     SALARY RANGE");
                    Console.WriteLine("--------------------------------------------------");

                    foreach (var pos in jobPositions)
                    {
                        Console.WriteLine($"{pos.Id,-3} {pos.PositionTitle,-17} {pos.Department.DepartmentName,-13} ${pos.MinSalary:N0}-${pos.MaxSalary:N0}");
                    }

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("Choose Job Position Id:");

                    if (!int.TryParse(Console.ReadLine(), out int jobPositionId))
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input!");
                        Console.ResetColor();
                        break;
                    }

                    var jobPosition = _jobPositionService.GetJobPositionByIdWithDetails(jobPositionId);
                    
                    if (jobPosition == null)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Job Position Not Found");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=== Search Employees ===");
                        Console.WriteLine($"Job Position: {jobPosition.PositionTitle}");
                        Console.WriteLine($"EMPLOYEES: {jobPosition.Employees.Count(e => e.IsActive)}");
                        Console.WriteLine("─".PadRight(70, '─'));

                        if (!jobPosition.Employees.Any(e => e.IsActive))
                        {
                            Console.WriteLine("No employees with this Job Position");
                            Console.WriteLine("─".PadRight(70, '─'));
                            Console.WriteLine("Click Any Key To Exit.");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }

                        Console.WriteLine("ID  NAME                      POSITION           EMAIL");
                        Console.WriteLine("─".PadRight(70, '─'));

                        foreach (var emp in jobPosition.Employees)
                        {
                            Console.WriteLine(
                                $"{emp.Id,-3} {emp.FirstName + " " + emp.LastName,-17}     {emp.JobPosition?.PositionTitle,-17} {emp.User.Email}");
                        }

                        Console.WriteLine("─".PadRight(70, '─'));
                        Console.WriteLine("Click Any Key To Exit.");
                        Console.ReadKey();
                        Console.Clear();
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