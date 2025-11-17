namespace HR_Payroll_managment_system.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    T GetById(int Id);
    List<T> GetAll();
    T Add(T entity);
    T Update(T entity);
    void Delete(int Id);
}