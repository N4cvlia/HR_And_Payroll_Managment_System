using HR_Payroll_managment_system.Interfaces;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Helpers;

public class Logging : ILogging
{
    private string _currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    
    public void LogEmail(EmailLog log)
    {
        string logsFolder = Path.Combine(_currentDirectory, "Logs");
        string emailLogPath = Path.Combine(logsFolder, "EmailLog.txt");
        
        using (StreamWriter sw = new StreamWriter(emailLogPath, true))
        {
            sw.WriteLine($"[{log.SentDate}] | Sent To: {log.ToEmail} | Subject: {log.Subject} | Body: {log.Body}");
        }
    }

    public void LogActivity(ActivityLog log)
    {
        string logsFolder = Path.Combine(_currentDirectory, "Logs");
        string activityLogPath = Path.Combine(logsFolder, "ActivityLog.txt");

        using (StreamWriter sw = new StreamWriter(activityLogPath, true))
        {
            sw.WriteLine($"[{log.TimeStamp}] | User: {log.User.Email} | Action: {log.Action} | Description: {log.Description}");
        }
    }
}