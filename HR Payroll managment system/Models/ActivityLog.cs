using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class ActivityLog : Base
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string Action {  get; set; }
    public string Description { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}