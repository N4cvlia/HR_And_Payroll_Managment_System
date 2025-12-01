using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IJobPositionRepository
{
    JobPosition GetById(int jobPositionId);
    JobPosition GetByIdWithDetails(int jobPositionId);
    List<JobPosition> GetAll();
    JobPosition Add(JobPosition jobPosition);
    JobPosition Update(JobPosition jobPosition);
    void Delete(int jobPositionId);
}