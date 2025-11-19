using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class EmployeeManagementMenu
{
    EmployeeService _employeeService;
    
    public void MainMenu()
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
                    ViewAllMenu();
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
    
    void ViewAllMenu()
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
}