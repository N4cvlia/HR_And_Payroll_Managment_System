using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories;
using HR_Payroll_managment_system.Services.Interfaces;

namespace HR_Payroll_managment_system.Services;

public class UserService : IUserService
{
    public User CurrentUser { get; set; }
    public bool IsLoggedIn => CurrentUser != null;
}