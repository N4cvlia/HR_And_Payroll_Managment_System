using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;

namespace HR_Payroll_managment_system.Services;

public class ActivityLogsService : IActivityLogsService
{
    ActivityLogsRepository  _activityLogsRepository = new ActivityLogsRepository();

    public void AddActivityLog(ActivityLog activityLog)
    {
        _activityLogsRepository.Add(activityLog);
    }
}