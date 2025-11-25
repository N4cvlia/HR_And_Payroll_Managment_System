namespace HR_Payroll_managment_system.DTOs;

public class AttendanceReportSummaryDto
{
    public string DepartmentName { get; set; }
    public int Present {  get; set; }
    public int Absent  { get; set; }
}