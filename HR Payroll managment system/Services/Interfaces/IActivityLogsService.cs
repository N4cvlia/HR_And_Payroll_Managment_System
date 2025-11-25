using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Services.Interfaces;

public interface IActivityLogsService
{
    void AddActivityLog(ActivityLog activityLog);
}