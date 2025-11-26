using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;

namespace HR_Payroll_managment_system.Services;

public class DeductionService : IDeductionService
{
    DeductionRepository  _deductionRepository =  new DeductionRepository();
    
    ActivityLogsService _activityLogsService = new ActivityLogsService();
    UserService _userService;
    
    Logging _logging = new Logging();

    public DeductionService(UserService userService)
    {
        _userService = userService;
    }
    
    public ValidationResult AddDeduction(Deduction deduction, EmployeeProfile employee)
    {
        var currentUser = _userService.CurrentUser;
        var payroll = employee.Payrolls.LastOrDefault();

        if (payroll == null) return new ValidationResult { Success = false };

        deduction.PaysrollId = payroll.Id;
        _deductionRepository.Add(deduction);

        var activityLog = new ActivityLog
        {
            UserId = currentUser.Id,
            Action = $"{currentUser.Email} Added a Bonus",
            Description = $"{currentUser.Email} Added a Bonus To a {employee.FirstName} {employee.LastName}"
        };

        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsService.AddActivityLog(activityLog);
        
        return  new ValidationResult { Success = true };
    }
}