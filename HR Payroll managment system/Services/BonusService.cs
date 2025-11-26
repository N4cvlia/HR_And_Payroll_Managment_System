using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;

namespace HR_Payroll_managment_system.Services;

public class BonusService : IBonusService
{
    BonusRepository _bonusRepository = new BonusRepository();
    
    ActivityLogsService _activityLogsService = new ActivityLogsService();
    UserService _userService;
    
    Logging _logging = new Logging();

    public BonusService(UserService userService)
    {
        _userService = userService;
    }

    public ValidationResult AddBonus(Bonus bonus, EmployeeProfile employee)
    {
        var currentUser = _userService.CurrentUser;
        var payroll = employee.Payrolls.LastOrDefault();

        if (payroll == null) return new ValidationResult { Success = false };

        bonus.PayrollId = payroll.Id;
        _bonusRepository.Add(bonus);

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