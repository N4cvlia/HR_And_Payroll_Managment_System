using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IBonusRepository
{
    Bonus GetById(int bonusId);
    List<Bonus> GetAll();
    Bonus Add(Bonus bonus);
    Bonus Update(Bonus bonus);
    void Delete(int bonusId);
}