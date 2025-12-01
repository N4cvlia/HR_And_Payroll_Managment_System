using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Presentation.Interfaces;

public interface IEmployeeManagementMenu
{
    void MainMenu();
    void ViewAllMenu();
    void EditMenu();
    void EditMenuOptionsMenu(EmployeeProfile employee);
    void AssignMenu();
    void IsActiveMenu();
    void ViewEmployeeMenu();
    void SearchMenu();
}