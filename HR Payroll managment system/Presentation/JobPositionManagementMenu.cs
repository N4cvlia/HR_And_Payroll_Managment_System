using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class JobPositionManagementMenu
{
    JobPositionService _jobPositionService;
    DepartmentService  _departmentService;

    public JobPositionManagementMenu(DepartmentService departmentService, JobPositionService jobPositionService)
    {
        _jobPositionService = jobPositionService;
        _departmentService = departmentService;
    }
    
    public void MainMenu()
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
                    ViewAllMenu();
                    break;
                case "2":
                    AddMenu();
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
    
    void ViewAllMenu()
    {
        Console.Clear();
        var allPositions = _jobPositionService.GetAllJobPositions()
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
    
    void AddMenu()
    {
        var departments = _departmentService.GetAllDepartmentsWithCount();

    if (departments.Count() < 2) return;
    
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
        Console.WriteLine(
            $"ID  {"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  {"DESCRIPTION".PadRight(descColumnWidth)}");
        Console.WriteLine("─".PadRight(totalWidth, '─'));

        foreach (var dept in departments)
        {
            Console.WriteLine(
                $"{dept.Id,-3} {dept.DepartmentName.PadRight(deptColumnWidth)} {dept.EmployeeCount,-10} {dept.Description?.PadRight(descColumnWidth)}");
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
        }
        else if (department.DepartmentName == "Employee")
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Job Position Cannot Be Assigned To Unassigned Department!");
            Console.ResetColor();
        }
        else
        {
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
    
            JobPosition newJobPosition = new JobPosition()
            {
                PositionTitle = positionTitle,
                Description = positionDescription,
                MinSalary = minSalary,
                MaxSalary = maxSalary,
                DepartmentId = departmentId
            };
            
            var result = _jobPositionService.AddJobPosition(newJobPosition);
            
            if (result.Success)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Created a New Job Position!");
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
            isRunning2 = false;
        }
    }while(isRunning2);
}
}