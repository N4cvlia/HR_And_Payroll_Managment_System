using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class EmailLog : Base
{
    public string ToEmail  { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime SentDate { get; set; } = DateTime.Now;
    public bool IsSent { get; set; }
    public string ErrorMessage  { get; set; }
}