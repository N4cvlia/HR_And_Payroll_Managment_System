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
        if (!pending.Any())
        {
            Console.WriteLine("No pending requests found.");
            Console.ReadKey();
            return;
        }
        
        Console.WriteLine("ID  EMPLOYEE              TYPE        FROM       TO       DAYS      STATUS");
        Console.WriteLine("---------------------------------------------------------------------------");

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
        
    }
}