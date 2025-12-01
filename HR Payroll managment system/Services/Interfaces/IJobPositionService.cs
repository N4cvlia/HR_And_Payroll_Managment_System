using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Services.Interfaces;

public interface IJobPositionService
{
    JobPosition GetJobPositionById(int id);
    JobPosition GetJobPositionByIdWithDetails(int id);
    List<JobPosition> GetAllJobPositions();
    ValidationResult AddJobPosition(JobPosition jobPosition);
    ValidationResult UpdateJobPosition(JobPosition jobPosition);
}