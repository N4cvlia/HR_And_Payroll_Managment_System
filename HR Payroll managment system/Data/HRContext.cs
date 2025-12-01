using HR_Payroll_managment_system.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Payroll_managment_system.Data;

public class HRContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<EmployeeProfile> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }
    
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    
    public DbSet<EmailLog> EmailLogs { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<Bonus> Bonuses { get; set; }
    public DbSet<Deduction> Deductions { get; set; }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=HRContext2;User Id=sa;Password=YourStrong@Passw0rd123;TrustServerCertificate=True;");
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                Id = 1,
                DepartmentName = "Unassigned",
                Description = "Employees pending assignment"
            },
            new Department()
            {
                Id = 2,
                DepartmentName = "HR Department",
                Description = "Human Rescource Management Team"
            }
        );
        modelBuilder.Entity<JobPosition>().HasData(
            new JobPosition
            {
                Id = 1,
                PositionTitle = "Employee",
                Description = "Employees pending Job Position assignment",
                DepartmentId = 1,
                MinSalary = 0,
                MaxSalary = 0
            },
            new JobPosition()
            {
                Id = 2,
                PositionTitle = "HR",
                Description = "Human Rescource Manager",
                DepartmentId = 2,
                MinSalary = 1500,
                MaxSalary = 3000
            }
            );
        modelBuilder.Entity<Role>().HasData(
            new Role()
            {
                Id = 1,
                RoleName = "Employee",
            },
            new Role()
            {
                Id = 2,
                RoleName = "HR"
            }
            );
        
        modelBuilder.Entity<EmployeeProfile>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeProfile>()
            .HasOne(e => e.JobPosition)
            .WithMany(j => j.Employees)
            .HasForeignKey(e => e.JobPositionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<EmployeeProfile>()
            .HasOne(e => e.User)
            .WithOne(u => u.EmployeeProfile)
            .HasForeignKey<EmployeeProfile>(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}