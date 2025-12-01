using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation.Interfaces;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class DepartmentManagementMenu : IDepartmentManagementMenu
{
    DepartmentService  _departmentService;

    public DepartmentManagementMenu(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
    
    // Menu Fucntions
    #region Menu Functions
    public void MainMenu()
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
                    ViewAllMenu();
                    break;
                case "2":
                    AddMenu();
                    break;
                case "3":
                    EditDetailsMenu();
                    break;
                case "4":
                    ViewDepartmentEmployees();
                    break;
                case "5":
                    SalaryReportsMenu();
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
    
    public void ViewAllMenu()
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

        Console.WriteLine("=== View All Departments ===");
        Console.WriteLine("─".PadRight(totalWidth, '─'));
        Console.WriteLine($"ID  {"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  {"DESCRIPTION".PadRight(descColumnWidth)}");
        Console.WriteLine("─".PadRight(totalWidth, '─'));

        foreach (var dept in allDepartments)
        {
            Console.WriteLine($"{dept.Id,-3} {dept.DepartmentName.PadRight(deptColumnWidth)} {dept.Count,-10} {dept.Description?.PadRight(descColumnWidth)}");
        }

        Console.WriteLine("─".PadRight(totalWidth, '─'));
        Console.WriteLine("Click Any Key To Exit.");
        Console.ReadKey();
        Console.Clear();
    }
    
    public void AddMenu()
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
        
        var result = _departmentService.AddDepartment(newDepartment);
        

        if (result.Success)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully added new department!");
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

    public void EditDetailsMenu()
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

        Console.WriteLine("=== Edit Department Details ===");
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

        var department = _departmentService.GetDepartmentById(departmentId);

        if (department == null)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Department Not Found");
            Console.ResetColor();
        }
        else if (department.Id == 1)
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
            var departmentName = Console.ReadLine();

            Console.WriteLine("Enter New Department Description:");
            var departmentDescription = Console.ReadLine();

            var newDepartment = new Department
            {
                DepartmentName = departmentName,
                Description = departmentDescription
            };

            var result = _departmentService.UpdateDepartment(department, newDepartment);

            if (result.Success)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully edited the department!");
                Console.ResetColor();
            }
            else
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var e in result.Errors) Console.WriteLine(e);
                Console.ResetColor();
            }
        }
    }
    
    public void ViewDepartmentEmployees()
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

        Console.WriteLine("=== View Department Employees ===");
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
                return;
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
    }
    
    public void SalaryReportsMenu()
    {
        Console.Clear();
        var report = _departmentService.GetAllDepartmentSalaryReports();
    
        int maxDeptNameLength = report.Max(d => d.DepartmentName.Length);
        int deptColumnWidth = Math.Max(maxDeptNameLength, 10) + 2;

        Console.WriteLine("=== DEPARTMENT SALARY REPORT ===");
        Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));
        Console.WriteLine($"{"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  AVG SALARY  TOTAL SALARY");
        Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));

        foreach (var dept in report)
        {
            Console.WriteLine($"{dept.DepartmentName.PadRight(deptColumnWidth)} {dept.EmployeeCount,-10} ${dept.AverageSalary,-10:N0} ${dept.TotalSalary:N0}");
        }

        var companyTotal = report.Sum(r => r.TotalSalary);
        var companyAvg = report.Average(r => r.AverageSalary);
        var totalEmployees = report.Sum(r => r.EmployeeCount);

        Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));
        Console.WriteLine($"{"TOTALS:".PadRight(deptColumnWidth)} {totalEmployees,-10} ${companyAvg,-10:N0} ${companyTotal:N0}");
        Console.WriteLine("─".PadRight(deptColumnWidth + 35, '─'));
        Console.WriteLine("Click Any Key To Exit.");
        Console.ReadKey();
        Console.Clear();
    }
    #endregion
}