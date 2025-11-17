using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class UserRepository : IRepository<User>
{
    HRContext _db = new HRContext();

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