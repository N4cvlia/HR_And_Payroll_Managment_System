using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Presentation.Interfaces;
using HR_Payroll_managment_system.Services;

namespace HR_Payroll_managment_system.Presentation;

public class HrMenu : IHrMenu
{
    EmployeeManagementMenu _employeeManagementMenu;
    DepartmentManagementMenu _departmentManagementMenu;
    JobPositionManagementMenu _jobPositionManagementMenu;
    Attendance_TimeTrackingMenu _attendanceTimeTrackingMenu;
    LeaveRequestManagementMenu _leaveRequestManagementMenu;
    PayrollManagementMenu _payrollManagementMenu;
    
    UserService _userService;
    DepartmentService _departmentService;

    public HrMenu(
        UserService userService,
        DepartmentService departmentService,
        JobPositionService jobPositionService,
        EmployeeService employeeService,
        AttendanceRecordService attendanceRecordService,
        LeaveRequestService leaveRequestService,
        BonusService bonusService,
        DeductionService deductionService,
        PayrollService payrollService
        )
    {
        _userService = userService;
        _departmentManagementMenu = new DepartmentManagementMenu(departmentService);
        _jobPositionManagementMenu = new JobPositionManagementMenu(departmentService, jobPositionService);
        _employeeManagementMenu = new EmployeeManagementMenu(employeeService, departmentService, jobPositionService);
        _attendanceTimeTrackingMenu =  new Attendance_TimeTrackingMenu(employeeService, attendanceRecordService);
        _leaveRequestManagementMenu = new LeaveRequestManagementMenu(leaveRequestService);
        _payrollManagementMenu = new PayrollManagementMenu(employeeService, bonusService, deductionService, payrollService, userService);
        
    }
    
    public void MainMenu()
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
            Console.WriteLine("7. Back to Main Menu");
            Console.WriteLine("Choose an Option:");
        
            string choice =  Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _employeeManagementMenu.MainMenu();
                    break;
                case "2":
                    _departmentManagementMenu.MainMenu();
                    break;
                case "3":
                    _jobPositionManagementMenu.MainMenu();
                    break;
                case "4":
                    _attendanceTimeTrackingMenu.MainMenu();
                    break;
                case "5":
                    _leaveRequestManagementMenu.MainMenu();
                    break;
                case "6":
                    _payrollManagementMenu.MainMenu();
                    break;
                case "7":
                    _userService.CurrentUser = new User();
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
}