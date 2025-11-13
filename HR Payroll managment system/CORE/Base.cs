namespace HR_Payroll_managment_system.CORE;

public class Base
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastUpdatedAt { get; set; }
}