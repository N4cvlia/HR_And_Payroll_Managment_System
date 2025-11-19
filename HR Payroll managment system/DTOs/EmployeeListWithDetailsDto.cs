namespace HR_Payroll_managment_system.DTOs;

public class EmployeeListWithDetailsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DepartmentName { get; set; }
    public string JobPositionName { get; set; }
    public string Email { get; set; }
}