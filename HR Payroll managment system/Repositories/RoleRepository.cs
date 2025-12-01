using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class RoleRepository : IRoleRepository
{
    HRContext _db = new HRContext();
    public Role GetById(int roleId)
    {
        return _db.Roles.FirstOrDefault(r => r.Id == roleId);
    }

    public List<Role> GetAll()
    {
        return _db.Roles.ToList();
    }

    public Role Add(Role role)
    {
        _db.Roles.Add(role);
        _db.SaveChanges();
        return role;
    }

    public Role Update(Role role)
    {
        _db.Roles.Update(role);
        _db.SaveChanges();
        return role;
    }

    public void Delete(int roleId)
    {
        _db.Roles.Remove(_db.Roles.FirstOrDefault(r => r.Id == roleId));
        _db.SaveChanges();
    }
}