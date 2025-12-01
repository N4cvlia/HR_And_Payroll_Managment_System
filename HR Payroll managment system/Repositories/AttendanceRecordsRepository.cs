using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HR_Payroll_managment_system.Repositories;

public class AttendanceRecordsRepository : IAttendanceRecordsRepository
{
    private HRContext _db = new HRContext();
    
    public AttendanceRecord GetById(int attendanceRecordId)
    {
        return _db.AttendanceRecords.FirstOrDefault(a => a.Id == attendanceRecordId);
    }

    public List<AttendanceRecord> GetAll()
    {
        return _db.AttendanceRecords.ToList();
    }

    public List<AttendanceRecord> GetAllWithEmployeeDetails()
    {
        return _db.AttendanceRecords.Include(a => a.Employee).ToList();
    }

    public AttendanceRecord Add(AttendanceRecord attendanceRecord)
    {
        _db.AttendanceRecords.Add(attendanceRecord);
        _db.SaveChanges();
        return attendanceRecord;
    }

    public AttendanceRecord Update(AttendanceRecord attendanceRecord)
    {
        _db.AttendanceRecords.Update(attendanceRecord);
        _db.SaveChanges();
        return attendanceRecord;
    }

    public void Delete(int attendanceRecordId)
    {
        _db.AttendanceRecords.Remove(_db.AttendanceRecords.FirstOrDefault(a => a.Id == attendanceRecordId));
        _db.SaveChanges();
    }
}