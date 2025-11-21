using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation.Interfaces;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class JobPositionManagementMenu : IJobPositionManagementMenu
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
                case "3":
                    EditMenu();
                    break;
                case "4":
                    AssignSalary();
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
    
    // Menu Fucntions
    #region Menu Functions
    public void ViewAllMenu()
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

    public void AddMenu()
    {
        var departments = _departmentService.GetAllDepartmentsWithCount();

        if (departments.Count() < 2)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Add More Departments First!");
            Console.ResetColor();
            return;
        }

        var isRunning2 = true;
        do
        {
            Console.Clear();
            var maxDeptNameLength = departments.Max(d => d.DepartmentName.Length);
            var deptColumnWidth = Math.Max(maxDeptNameLength, 10) + 2;

            var maxDescLength = departments.Max(d => d.Description?.Length ?? 0);
            var descColumnWidth = Math.Max(maxDescLength, 11) + 2;

            var totalWidth = deptColumnWidth + descColumnWidth + 20;

            Console.WriteLine("=== Create New Positions ===");
            Console.WriteLine("─".PadRight(totalWidth, '─'));
            Console.WriteLine(
                $"ID  {"DEPARTMENT".PadRight(deptColumnWidth)} EMPLOYEES  {"DESCRIPTION".PadRight(descColumnWidth)}");
            Console.WriteLine("─".PadRight(totalWidth, '─'));

            foreach (var dept in departments)
                Console.WriteLine(
                    $"{dept.Id,-3} {dept.DepartmentName.PadRight(deptColumnWidth)} {dept.EmployeeCount,-10} {dept.Description?.PadRight(descColumnWidth)}");

            Console.WriteLine("─".PadRight(totalWidth, '─'));
            Console.WriteLine("Choose a Department Id: ");

            if (!int.TryParse(Console.ReadLine(), out var departmentId))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input!");
                Console.ResetColor();
                return;
            }

            var department = departments.FirstOrDefault(d => d.Id == departmentId);

            if (department == null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No Department Found!");
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
                var positionTitle = Console.ReadLine();
                Console.WriteLine("Enter Position Description:");
                var positionDescription = Console.ReadLine();
                Console.WriteLine("Enter Min Salary:");
                var minSalary = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Enter Max Salary:");
                var maxSalary = decimal.Parse(Console.ReadLine());

                var newJobPosition = new JobPosition
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
                    foreach (var e in result.Errors) Console.WriteLine(e);
                    Console.ResetColor();
                }

                isRunning2 = false;
            }
        } while (isRunning2);
    }

    public void EditMenu()
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

        Console.WriteLine("=== Edit Position Details ===");
        Console.WriteLine("─".PadRight(totalWidth, '─'));
        Console.WriteLine($"ID  {"POSITION".PadRight(titleColumnWidth)} {"DEPARTMENT".PadRight(deptColumnWidth)} SALARY RANGE");
        Console.WriteLine("─".PadRight(totalWidth, '─'));

        foreach (var pos in allPositions)
        {
            string salaryRange = $"${pos.MinSalary:N0}-${pos.MaxSalary:N0}";
            Console.WriteLine($"{pos.Id,-3} {pos.PositionTitle.PadRight(titleColumnWidth)} {pos.Department.PadRight(deptColumnWidth)} {salaryRange}");
        }

        Console.WriteLine("─".PadRight(totalWidth, '─'));
        Console.WriteLine("Choose Job Position Id:");

        if (!int.TryParse(Console.ReadLine(), out var jobPositionId))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }

        var jobPosition = _jobPositionService.GetJobPositionById(jobPositionId);

        if (jobPosition == null)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Job Position Found!");
            Console.ResetColor();
        }
        else if (jobPositionId == 1)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cannot Edit UnAssigned Position!");
            Console.ResetColor();
        }
        else
        {
            Console.Clear();

            bool isRunning = true;

            Console.WriteLine("=== Edit Position Details ===");
            Console.WriteLine("1. Edit Job Position Title");
            Console.WriteLine("2. Edit Job Position Description");
            Console.WriteLine("3. Back To HR Menu");
            Console.WriteLine("Choose An Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("=== Edit Position Details ===");
                    Console.WriteLine("Enter New Job Position Title:");
                    string newJobPositionTitle = Console.ReadLine();
                    
                    jobPosition.PositionTitle = newJobPositionTitle;
                    
                    var result =  _jobPositionService.UpdateJobPosition(jobPosition);

                    if (result.Success)
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Edited Job Position Title!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (var e in result.Errors) Console.WriteLine(e);
                        Console.ResetColor();
                    }
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("=== Edit Position Details ===");
                    Console.WriteLine("Enter New Job Position Description:");
                    string newJobPositionDesc = Console.ReadLine();
                    
                    jobPosition.Description = newJobPositionDesc;
                    
                    var result2 =  _jobPositionService.UpdateJobPosition(jobPosition);

                    if (result2.Success)
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Edited Job Position Description!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Red;
                        foreach (var e in result2.Errors) Console.WriteLine(e);
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
            
            
        }
    }

    public void AssignSalary()
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

        Console.WriteLine("=== Assign Salary Ranges ===");
        Console.WriteLine("─".PadRight(totalWidth, '─'));
        Console.WriteLine($"ID  {"POSITION".PadRight(titleColumnWidth)} {"DEPARTMENT".PadRight(deptColumnWidth)} SALARY RANGE");
        Console.WriteLine("─".PadRight(totalWidth, '─'));

        foreach (var pos in allPositions)
        {
            string salaryRange = $"${pos.MinSalary:N0}-${pos.MaxSalary:N0}";
            Console.WriteLine($"{pos.Id,-3} {pos.PositionTitle.PadRight(titleColumnWidth)} {pos.Department.PadRight(deptColumnWidth)} {salaryRange}");
        }

        Console.WriteLine("─".PadRight(totalWidth, '─'));
        Console.WriteLine("Choose Job Position Id:");

        if (!int.TryParse(Console.ReadLine(), out var jobPositionId))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }

        var jobPosition = _jobPositionService.GetJobPositionById(jobPositionId);

        if (jobPosition == null)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Job Position Found!");
            Console.ResetColor();
        }
        else if (jobPositionId == 1)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cannot Edit UnAssigned Position!");
            Console.ResetColor();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("=== Assign Salary Ranges ===");
            Console.WriteLine("Enter New Minimum Salary:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal minSalary))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Enter New Maximum Salary:");
            if (!decimal.TryParse(Console.ReadLine(), out decimal maxSalary))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input!");
                Console.ResetColor();
                return;
            }
            
            jobPosition.MinSalary = minSalary;
            jobPosition.MaxSalary = maxSalary;
            
            var result =  _jobPositionService.UpdateJobPosition(jobPosition);

            if (result.Success)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Edited Job Position Salary Range!");
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
    #endregion
}