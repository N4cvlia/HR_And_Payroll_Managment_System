using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Services;

public interface IAttendanceRecordService
{
    List<AttendanceRecord> GetAll();
    List<AttendanceRecord> GetAllWithEmployeeDetails();
    bool CheckIn();
    bool CheckOut();
    bool CheckInAsHr(int id);
    bool CheckOutAsHR(int id);
    void ExportTimesheetToPDF(EmployeeProfile employee, DateTime fromDate, DateTime toDate);
}