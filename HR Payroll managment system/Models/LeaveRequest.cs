using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class LeaveRequest : Base
{
    public int EmployeeId { get; set; }
    public EmployeeProfile Employee { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LeaveType { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime RequestDate { get; set; } = DateTime.Now;
    public string? AdminComments { get; set; }
}