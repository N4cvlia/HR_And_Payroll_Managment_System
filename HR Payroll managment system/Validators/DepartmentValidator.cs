using FluentValidation;
using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Validators;

public class DepartmentValidator : AbstractValidator<Department>
{
    public DepartmentValidator()
    {
        RuleFor(d => d.DepartmentName)
            .NotEmpty().WithMessage("Department name cannot be empty")
            .MaximumLength(50).WithMessage("Department name cannot be longer than 50 characters")
            .Must(DepartmentExists).WithMessage("Department already exists");
        
        RuleFor(d => d.Description)
            .NotEmpty().WithMessage("Department description cannot be empty")
            .MaximumLength(50).WithMessage("Department description cannot be longer than 50 characters");
    }
    private bool DepartmentExists(string departmentName)
    {
        using var context = new HRContext();
        return !context.Departments.Any(x => x.DepartmentName == departmentName);
    }
}