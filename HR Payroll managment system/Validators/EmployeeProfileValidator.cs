using FluentValidation;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Validators;

public class EmployeeProfileValidator : AbstractValidator<EmployeeProfile>
{
    public EmployeeProfileValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters")
            .MinimumLength(2).WithMessage("First name cannot be less than 2 characters");
        RuleFor(e => e.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters")
            .MinimumLength(2).WithMessage("Last name cannot be less than 2 characters");

    }
}