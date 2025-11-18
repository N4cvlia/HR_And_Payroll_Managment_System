using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class ActivityLogsRepository : IRepository<ActivityLog>
{
    private HRContext _db = new HRContext();
    
    public ActivityLog GetById(int activityLogId)
    {
        return _db.ActivityLogs.FirstOrDefault(u => u.Id == activityLogId);
    }

    public List<ActivityLog> GetAll()
    {
        return _db.ActivityLogs.ToList();
    }

    public ActivityLog Add(ActivityLog activityLog)
    {
        _db.ActivityLogs.Add(activityLog);
        _db.SaveChanges();
        return activityLog;
    }

    public ActivityLog Update(ActivityLog activityLog)
    {
        _db.ActivityLogs.Update(activityLog);
        _db.SaveChanges();
        return activityLog;
    }

    public void Delete(int activityLogId)
    {
        _db.ActivityLogs.Remove(_db.ActivityLogs.FirstOrDefault(a => a.Id == activityLogId));
        _db.SaveChanges();
    }
}