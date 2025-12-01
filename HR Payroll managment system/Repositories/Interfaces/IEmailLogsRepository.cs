using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IEmailLogsRepository
{
    EmailLog GetById(int emailLogId);
    List<EmailLog> GetAll();
    EmailLog Add(EmailLog emailLog);
    EmailLog Update(EmailLog emailLog);
    void Delete(int emailLogId);
}