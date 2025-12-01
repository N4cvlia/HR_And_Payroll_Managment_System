using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IRoleRepository
{
    Role GetById(int roleId);
    List<Role> GetAll();
    Role Add(Role role);
    Role Update(Role role);
    void Delete(int roleId);
}