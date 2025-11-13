using HR_Payroll_managment_system.CORE;

namespace HR_Payroll_managment_system.Models;

public class AttendanceRecord : Base
{
    public int EmployeeId { get; set; }
    public EmployeeProfile Employee { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public DateTime WorkDate { get; set; }
    public decimal HoursWorked => CalculateHoursWorked();

    private decimal CalculateHoursWorked()
    {
        if (CheckOut.HasValue)
            return (decimal)(CheckOut.Value - CheckIn).TotalHours;
        return 0;
    }
}