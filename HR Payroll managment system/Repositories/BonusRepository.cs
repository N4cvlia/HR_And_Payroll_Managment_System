using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;
using HR_Payroll_managment_system.Repositories.Interfaces;

namespace HR_Payroll_managment_system.Repositories;

public class BonusRepository : IBonusRepository
{
    HRContext _db = new HRContext();
    
    public Bonus GetById(int bonusId)
    {
        return _db.Bonuses.FirstOrDefault(b => b.Id == bonusId);
    }

    public List<Bonus> GetAll()
    {
        return _db.Bonuses.ToList();
    }

    public Bonus Add(Bonus bonus)
    {
        _db.Bonuses.Add(bonus);
        _db.SaveChanges();
        return bonus;
    }

    public Bonus Update(Bonus bonus)
    {
        _db.Bonuses.Update(bonus);
        _db.SaveChanges();
        return bonus;
    }

    public void Delete(int bonusId)
    {
        _db.Bonuses.Remove(_db.Bonuses.FirstOrDefault(u => u.Id == bonusId));
        _db.SaveChanges();
    }
}