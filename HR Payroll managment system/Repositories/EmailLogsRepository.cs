using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class EmailLogsRepository : IRepository<EmailLog>
{
    private HRContext _db = new HRContext();
    
    public EmailLog GetById(int emailLogId)
    {
        return _db.EmailLogs.FirstOrDefault(e => e.Id == emailLogId);
    }

    public List<EmailLog> GetAll()
    {
        return _db.EmailLogs.ToList();
    }

    public EmailLog Add(EmailLog emailLog)
    {
        _db.EmailLogs.Add(emailLog);
        _db.SaveChanges();
        return emailLog;
    }

    public EmailLog Update(EmailLog emailLog)
    {
        _db.EmailLogs.Update(emailLog);
        _db.SaveChanges();
        return emailLog;
    }

    public void Delete(int emailLogId)
    {
        _db.EmailLogs.Remove(_db.EmailLogs.FirstOrDefault(e => e.Id == emailLogId));
        _db.SaveChanges();
    }
}