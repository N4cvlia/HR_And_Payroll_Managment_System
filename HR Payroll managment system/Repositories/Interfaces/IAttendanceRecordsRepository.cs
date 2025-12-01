using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IAttendanceRecordsRepository
{
    List<AttendanceRecord> GetAll();
    List<AttendanceRecord> GetAllWithEmployeeDetails();
    AttendanceRecord Add(AttendanceRecord attendanceRecord);
    AttendanceRecord Update(AttendanceRecord attendanceRecord);
    void Delete(int attendanceRecordId);
}