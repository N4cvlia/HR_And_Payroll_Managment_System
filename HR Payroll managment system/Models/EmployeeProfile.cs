using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class EmployeeProfile : Base
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime HireDate { get; set; }
    public decimal BaseSalary { get; set; } = 0;
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public int JobPositionId { get; set; }
    public JobPosition JobPosition { get; set; }
    public bool IsActive { get; set; } = true;
    
    public List<AttendanceRecord>  AttendanceRecords { get; set; }
    public List<LeaveRequest>  LeaveRequests { get; set; }
    public List<Payroll>  Payrolls { get; set; }
}