using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;
using HR_Payroll_managment_system.Validators;

namespace HR_Payroll_managment_system.Services;

public class JobPositionService : IJobPositionService
{
    JobPositionRepository _jobPositionRepository = new JobPositionRepository();
    ActivityLogsRepository _activityLogsRepository = new ActivityLogsRepository();
    
    JobPositionValidator _jobPositionValidator = new JobPositionValidator();
    
    UserService _userService;
    
    Logging _logging =  new Logging();

    public JobPositionService(UserService userService)
    {
        _userService = userService;
    }

    public JobPosition GetJobPositionById(int id)
    {
        return  _jobPositionRepository.GetById(id);
    }

    public JobPosition GetJobPositionByIdWithDetails(int id)
    {
        return _jobPositionRepository.GetByIdWithDetails(id);
    }

    public List<JobPosition> GetAllJobPositions()
    {
        return _jobPositionRepository.GetAll();
    }

    public ValidationResult AddJobPosition(JobPosition jobPosition)
    {
        var isValidJobPosition = _jobPositionValidator.Validate(jobPosition);
        
        if (!isValidJobPosition.IsValid)
            return new ValidationResult
            {
                Success = false,
                Errors = isValidJobPosition.Errors.Select(e => e.ErrorMessage).ToList()
            };

        var currentUser = _userService.CurrentUser;

        _jobPositionRepository.Add(jobPosition);
        
        ActivityLog activityLog = new ActivityLog()
        {
            UserId = currentUser.Id,
            Action = "Job Position Creation",
            Description = $"{jobPosition.PositionTitle} Was Created"
        };
        
        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsRepository.Add(activityLog);

        return new ValidationResult
        {
            Success = true,
        };
    }
}