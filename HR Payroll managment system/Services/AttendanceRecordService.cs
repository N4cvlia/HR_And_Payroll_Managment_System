using HR_Payroll_managment_system.DTOs;
using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;

namespace HR_Payroll_managment_system.Services;

public class AttendanceRecordService : IAttendanceRecordService
{
    AttendanceRecordsRepository _attendanceRecordsRepository =  new AttendanceRecordsRepository();
    ActivityLogsRepository _activityLogsRepository =  new ActivityLogsRepository();
    DepartmentRepository _departmentRepository =  new DepartmentRepository();
    
    PDFHelper  _pdfHelper =  new PDFHelper();
    
    UserService _userService;
    
    Logging _logging = new Logging();

    public AttendanceRecordService(UserService userService)
    {
        _userService = userService;
    }

    public List<AttendanceRecord> GetAll()
    {
        return _attendanceRecordsRepository.GetAll();
    }
    
    public List<AttendanceRecord> GetAllWithEmployeeDetails()
    {
        return _attendanceRecordsRepository.GetAllWithEmployeeDetails();
    }
    
    public bool CheckIn()
    {
        var currentUser = _userService.CurrentUser;
        var result = _attendanceRecordsRepository.GetAll().Where(a => a.EmployeeId == currentUser.EmployeeProfile.Id).FirstOrDefault(a => a.WorkDate == DateTime.Today);

        if (result == null)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord()
            {
                EmployeeId = currentUser.EmployeeProfile.Id,
                CheckIn = DateTime.Now,
                WorkDate = DateTime.Today
            };

            ActivityLog activityLog = new ActivityLog()
            {
                UserId = currentUser.Id,
                Action = "Checked In",
                Description = "User Checked Into The Work"
            };
            
            _logging.LogActivity(activityLog, currentUser.Email);
            _activityLogsRepository.Add(activityLog);
            _attendanceRecordsRepository.Add(attendanceRecord);
            
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckOut()
    {
        var currentUser = _userService.CurrentUser;
        var result = _attendanceRecordsRepository.GetAll().Where(a => a.EmployeeId == currentUser.EmployeeProfile.Id).FirstOrDefault(a => a.WorkDate == DateTime.Today);
        
        if (result != null && result.CheckOut == null)
        {
            result.CheckOut = DateTime.Now;
            
            ActivityLog activityLog = new ActivityLog()
            {
                UserId = currentUser.Id,
                Action = "Checked Out",
                Description = "User Checked Out Of The Work"
            };
            
            _logging.LogActivity(activityLog, currentUser.Email);
            _activityLogsRepository.Add(activityLog);
            _attendanceRecordsRepository.Update(result);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckInAsHr(int id)
    {
        if (_userService.CurrentUser.EmployeeProfile.Id == id)
        {
            return CheckIn();
        }
        
        var result = _attendanceRecordsRepository.GetAll().Where(a => a.EmployeeId == id).FirstOrDefault(a => a.WorkDate == DateTime.Today);

        var currentUser = _userService.CurrentUser;
        if (result == null)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord()
            {
                EmployeeId = id,
                CheckIn = DateTime.Now,
                WorkDate = DateTime.Today
            };

            ActivityLog activityLog = new ActivityLog()
            {
                UserId = id,
                Action = "Checked In",
                Description = "User Checked Into The Work"
            };
            
            _logging.LogActivity(activityLog, currentUser.Email);
            _activityLogsRepository.Add(activityLog);
            _attendanceRecordsRepository.Add(attendanceRecord);
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckOutAsHR(int id)
    {
        if (_userService.CurrentUser.EmployeeProfile.Id == id)
        {
            return CheckOut();
        }
        var result = _attendanceRecordsRepository.GetAll().Where(a => a.EmployeeId == id).FirstOrDefault(a => a.WorkDate == DateTime.Today);

        var currentUser = _userService.CurrentUser;

        if (result != null && result.CheckOut == null)
        {
            result.CheckOut = DateTime.Now;
            
            ActivityLog activityLog = new ActivityLog()
            {
                UserId = id,
                Action = "Checked Out",
                Description = "User Checked Out Of The Work"
            };
            
            _logging.LogActivity(activityLog, currentUser.Email);
            _activityLogsRepository.Add(activityLog);
            _attendanceRecordsRepository.Update(result);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ExportTimesheetToPDF(EmployeeProfile employee, DateTime fromDate, DateTime toDate)
    {
        var currentUser = _userService.CurrentUser;

        if (fromDate > toDate)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("From Date cannot be greater than To Date");
            Console.ResetColor();
        }
        else
        {
        _pdfHelper.ExportTimesheetToPDF(employee, currentUser, fromDate, toDate);
        }
    }

    public List<AttendanceReportSummaryDto> GetDailyReport()
    {
        var departments = _departmentRepository.GetAllDepartmentsWithEmployeeAndAttendance();

        List<AttendanceReportSummaryDto> result = [];
        foreach (var dep in departments)
        {
            int present = 0;
            int totalEmployees = dep.Employees.Count;

            foreach (var emp in dep.Employees)
            {
                bool isPresent = emp.AttendanceRecords.Any(at => at.WorkDate == DateTime.Today);
                if (isPresent)
                {
                    present++;
                }
            }
            int absent = totalEmployees - present;
            
            result.Add(new AttendanceReportSummaryDto{ Absent = absent, Present = present , DepartmentName = dep.DepartmentName});
        }
        return result;
    }
    public List<AttendanceReportSummaryDto> GetMonthlyReport(int year, int month)
    {
        var departments = _departmentRepository.GetAllDepartmentsWithEmployeeAndAttendance();
    
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        List<AttendanceReportSummaryDto> result = [];
        foreach (var dep in departments)
        {
            int presentEmployees = 0;

            foreach (var emp in dep.Employees)
            {
                bool attendedThisMonth = emp.AttendanceRecords
                    .Any(at => at.WorkDate >= startDate && at.WorkDate <= endDate);
                
                if (attendedThisMonth)
                {
                    presentEmployees++;
                }
            }
        
            int absentEmployees = dep.Employees.Count - presentEmployees;
        
            result.Add(new AttendanceReportSummaryDto 
            { 
                Absent = absentEmployees, 
                Present = presentEmployees, 
                DepartmentName = dep.DepartmentName 
            });
        }
        return result;
    }
}