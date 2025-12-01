using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface ILeaveRequestRepository
{
    LeaveRequest GetById(int leaveRequestId);
    List<LeaveRequest> GetOnlyPending();
    List<LeaveRequest> GetAll();
    LeaveRequest Add(LeaveRequest leaveRequest);
    LeaveRequest Update(LeaveRequest leaveRequest);
    void Delete(int leaveRequestId);
}