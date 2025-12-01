using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HR_Payroll_managment_system.Repositories;

public class UserRepository : IUserRepository
{
    HRContext _db = new HRContext();
    
    public void AddProfileToUser(User user, EmployeeProfile employeeProfile)
    {
        user.EmployeeProfile = employeeProfile;
        _db.SaveChanges();
    }
    public User GetUserByEmail(string email)
    {
        return _db.Users
            .Include(u => u.Role)
            .Include(u => u.EmployeeProfile)
            .FirstOrDefault(u => u.Email == email);
    }
    public User GetById(int userId)
    {
        return _db.Users.FirstOrDefault(u => u.Id == userId);
    }

    public List<User> GetAll()
    {
        return _db.Users.ToList();
    }

    public User Add(User user)
    {
        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }

    public User Update(User user)
    {
        _db.Users.Update(user);
        _db.SaveChanges();
        return user;
    }

    public void Delete(int userId)
    {
        _db.Users.Remove(_db.Users.FirstOrDefault(u => u.Id == userId));
        _db.SaveChanges();
    }
}