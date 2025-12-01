using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Services.Interfaces;

public interface ILeaveRequestService
{
    ValidationResult AddLeaveRequest(LeaveRequest leaveRequest);
    List<LeaveRequest> GetAllRequests();
    List<LeaveRequest> GetOnlyPending();
    void UpdateLeaveRequest(LeaveRequest leaveRequest);
}