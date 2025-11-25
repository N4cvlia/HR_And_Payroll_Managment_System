using HR_Payroll_managment_system.Helpers;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;
using HR_Payroll_managment_system.Validators;

namespace HR_Payroll_managment_system.Services;

public class LeaveRequestService : ILeaveRequestService
{
    LeaveRequestRepository _leaveRequestRepository = new LeaveRequestRepository();
    ActivityLogsService _activityLogsService = new ActivityLogsService();
    
    UserService _userService;
    
    Logging _logging = new Logging();
    
    LeaveRequestValidator _leaveRequestValidator = new LeaveRequestValidator();

    public LeaveRequestService(UserService userService)
    {
        _userService = userService;
    }

    public ValidationResult AddLeaveRequest(LeaveRequest leaveRequest)
    {
        var isValidLeaveRequest = _leaveRequestValidator.Validate(leaveRequest);

        if (!isValidLeaveRequest.IsValid)
            return new ValidationResult
            {
                Success = false,
                Errors = isValidLeaveRequest.Errors.Select(e => e.ErrorMessage).ToList()
            };

        var currentUser = _userService.CurrentUser;

        _leaveRequestRepository.Add(leaveRequest);

        ActivityLog activityLog = new ActivityLog()
        {
            UserId = currentUser.Id,
            Action = "Leave Request Submitted",
            Description = $"{currentUser.Email} Has Submitted Leave Request"
        };
        
        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsService.AddActivityLog(activityLog);

        return new ValidationResult
        {
            Success = true
        };
    }

    public List<LeaveRequest> GetAllRequests()
    {
        return _leaveRequestRepository.GetAll();
    }
    public List<LeaveRequest> GetOnlyPending()
    {
        return _leaveRequestRepository.GetOnlyPending();
    }

    public void UpdateLeaveRequest(LeaveRequest leaveRequest)
    {
        var currentUser = _userService.CurrentUser;
        
        _leaveRequestRepository.Update(leaveRequest);

        ActivityLog activityLog = new ActivityLog()
        {
            UserId = currentUser.Id,
            Action = "Leave Request Updated",
            Description = $"{currentUser.Email} Has Updated Leave Request"
        };
        
        _logging.LogActivity(activityLog, currentUser.Email);
        _activityLogsService.AddActivityLog(activityLog);
    }
}