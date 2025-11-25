using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation.Interfaces;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class LeaveRequestManagementMenu : ILeaveRequestManagementMenu
{
    LeaveRequestService _leaveRequestService;

    public LeaveRequestManagementMenu(LeaveRequestService leaveRequestService)
    {
        _leaveRequestService = leaveRequestService;
    }

    // Menu Fucntions
    #region Menu Functions
    public void MainMenu()
    {
        var isRunning = true;

        Console.Clear();

        do
        {
            Console.WriteLine("=== Manage Leave Requests ===");
            Console.WriteLine("1. View Pending Requests");
            Console.WriteLine("2. Approve/Reject Requests");
            Console.WriteLine("3. View All Leave Requests");
            Console.WriteLine("4. Leave Reports");
            Console.WriteLine("5. Back To HR Menu");
            Console.WriteLine("Choose An Option:");

            switch (Console.ReadLine())
            {
                case "1":
                    ViewPendingRequestsMenu();
                    break;
                case "2":
                    HandleRequestsMenu();
                    break;
                case "3":
                    ViewAllRequestsMenu();
                    break;
                case "4":
                    LeaveReportMenu();
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

    public void ViewPendingRequestsMenu()
    {
        Console.Clear();
        var pending = _leaveRequestService.GetOnlyPending();

        Console.WriteLine("=== Manage Leave Requests ===");
        Console.WriteLine("ID  EMPLOYEE              TYPE        FROM       TO       DAYS      STATUS");
        Console.WriteLine("---------------------------------------------------------------------------");
        
        if (!pending.Any())
        {
            Console.WriteLine("No pending requests found.");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        foreach (var request in pending)
            Console.WriteLine(
                $"{request.Id,-3} {request.Employee.FirstName + " " + request.Employee.LastName,-15}    {request.LeaveType,-10}  {request.StartDate:MM/dd}     {request.EndDate:MM/dd}     {(request.EndDate - request.StartDate).Days + 1,-4}     {request.Status, -10}");

        Console.WriteLine("---------------------------------------------------------------------------");
        Console.WriteLine($"Found {pending.Count} pending request(s)");
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    public void HandleRequestsMenu()
    {
        Console.Clear();
        var pending = _leaveRequestService.GetOnlyPending();

        Console.WriteLine("=== Manage Leave Requests ===");
        Console.WriteLine("ID  EMPLOYEE              TYPE        FROM       TO       DAYS      STATUS");
        Console.WriteLine("---------------------------------------------------------------------------");
        
        if (!pending.Any())
        {
            Console.WriteLine("No pending requests found.");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        foreach (var request in pending)
            Console.WriteLine(
                $"{request.Id,-3} {request.Employee.FirstName + " " + request.Employee.LastName,-15}    {request.LeaveType,-10}  {request.StartDate:MM/dd}     {request.EndDate:MM/dd}     {(request.EndDate - request.StartDate).Days + 1,-4}     {request.Status, -10}");

        Console.WriteLine("---------------------------------------------------------------------------");
        Console.WriteLine("Choose Request Id:");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }
        
        var result = pending.FirstOrDefault(r => r.Id == id);

        if (result == null)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No pending request found.");
            Console.ResetColor();
        }
        else
        {
            bool isRunning2 = true;
            Console.Clear();

            do
            {
                Console.WriteLine("=== Manage Leave Requests ===");
                Console.WriteLine("1. Approve");
                Console.WriteLine("2. Reject");
                Console.WriteLine("3. Back To HR Menu");
                Console.WriteLine("Choose An Option:");
                
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        result.Status = "Approved";
                        _leaveRequestService.UpdateLeaveRequest(result);
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Approved Request!");
                        Console.ResetColor();
                        isRunning2 = false;
                        break;
                    case "2":
                        Console.Clear();
                        result.Status = "Rejected";
                        _leaveRequestService.UpdateLeaveRequest(result);
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully Rejected Request!");
                        Console.ResetColor();
                        isRunning2 = false;
                        break;
                    case "3":
                        Console.Clear();
                        isRunning2 = false;
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input!");
                        Console.ResetColor();
                        break;
                }
            } while (isRunning2);
            
        }
    }

    public void ViewAllRequestsMenu()
    {
        Console.Clear();
        var pending = _leaveRequestService.GetAllRequests();

        Console.WriteLine("=== Manage Leave Requests ===");
        Console.WriteLine("ID  EMPLOYEE              TYPE        FROM       TO       DAYS      STATUS");
        Console.WriteLine("---------------------------------------------------------------------------");
        
        if (!pending.Any())
        {
            Console.WriteLine("No leave requests found.");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        foreach (var request in pending)
            Console.WriteLine(
                $"{request.Id,-3} {request.Employee.FirstName + " " + request.Employee.LastName,-15}    {request.LeaveType,-10}  {request.StartDate:MM/dd}     {request.EndDate:MM/dd}     {(request.EndDate - request.StartDate).Days + 1,-4}     {request.Status, -10}");

        Console.WriteLine("---------------------------------------------------------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    public void LeaveReportMenu()
    {
        Console.Clear();
        var requests = _leaveRequestService.GetAllRequests();

        Console.WriteLine("=== Manage Leave Requests ===");
        Console.WriteLine("ID  EMPLOYEE              TYPE        FROM       TO       DAYS      STATUS");
        Console.WriteLine("---------------------------------------------------------------------------");
        
        if (!requests.Any())
        {
            Console.WriteLine("No leave requests found.");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        foreach (var request in requests)
            Console.WriteLine(
                $"{request.Id,-3} {request.Employee.FirstName + " " + request.Employee.LastName,-15}    {request.LeaveType,-10}  {request.StartDate:MM/dd}     {request.EndDate:MM/dd}     {(request.EndDate - request.StartDate).Days + 1,-4}     {request.Status, -10}");

        Console.WriteLine("---------------------------------------------------------------------------");
        Console.WriteLine("Choose Leave Request Id:");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input!");
            Console.ResetColor();
            return;
        }
        
        var result = requests.FirstOrDefault(r => r.Id == id);

        if (result == null)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No leave request found.");
            Console.ResetColor();
        }
        else
        {
            Console.Clear();
            
            Console.WriteLine("=== Manage Leave Requests ===");
            Console.WriteLine("Enter Your Comment:");
            string comment = Console.ReadLine();
            
            result.AdminComments = comment;
            _leaveRequestService.UpdateLeaveRequest(result);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully Added a Comment!");
            Console.ResetColor();
        }
    }
    #endregion
}