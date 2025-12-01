using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IUserRepository
{
    void AddProfileToUser(User user, EmployeeProfile employeeProfile);
    User GetUserByEmail(string email);
    User GetById(int userId);
    List<User> GetAll();
    User Add(User user);
    User Update(User user);
    void Delete(int userId);
}