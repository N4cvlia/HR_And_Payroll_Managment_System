using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IActivityLogsRepository
{
    List<ActivityLog> GetAll();
    ActivityLog Add(ActivityLog activityLog);
    ActivityLog Update(ActivityLog activityLog);
    void Delete(int activityLogId);

}