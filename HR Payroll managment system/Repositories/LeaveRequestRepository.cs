using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class LeaveRequestRepository : IRepository<LeaveRequest>
{
    private HRContext _db = new HRContext();
    
    
    public LeaveRequest GetById(int leaveRequestId)
    {
        return _db.LeaveRequests.FirstOrDefault(l => l.Id == leaveRequestId);
    }

    public List<LeaveRequest> GetAll()
    {
        return _db.LeaveRequests.ToList();
    }

    public LeaveRequest Add(LeaveRequest leaveRequest)
    {
        _db.LeaveRequests.Add(leaveRequest);
        _db.SaveChanges();
        return leaveRequest;
    }

    public LeaveRequest Update(LeaveRequest leaveRequest)
    {
        _db.LeaveRequests.Update(leaveRequest);
        _db.SaveChanges();
        return leaveRequest;
    }

    public void Delete(int leaveRequestId)
    {
        _db.LeaveRequests.Remove(_db.LeaveRequests.FirstOrDefault(l => l.Id == leaveRequestId));
        _db.SaveChanges();
    }
}