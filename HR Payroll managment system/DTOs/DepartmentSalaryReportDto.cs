namespace HR_Payroll_managment_system.DTOs;

public class DepartmentSalaryReportDto
{
    public string DepartmentName { get; set; }
    public int EmployeeCount { get; set; }
    public decimal TotalSalary { get; set; }
    public decimal AverageSalary { get; set; }
}