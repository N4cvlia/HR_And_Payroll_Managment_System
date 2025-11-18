using FluentValidation;
using HR_Payroll_managment_system.Data;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Validators;

public class LoginValidator : AbstractValidator<User>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
            .Must(ContainUpperCase).WithMessage("Password must contain at least one upper case letter")
            .Must(ContainLowerCase).WithMessage("Password must contain at least one lower case letter")
            .Must(ContainDigit).WithMessage("Password must contain at least one digit")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character (@$!%*?&)");
    }
    
    private bool ContainUpperCase(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper);
    }

    private bool ContainLowerCase(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Any(char.IsLower);
    }

    private bool ContainDigit(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Any(char.IsDigit);
    }

    private bool ContainSpecialCharacter(string password)
    {
        if (string.IsNullOrEmpty(password)) return false;
        var specialCharacters = "@$!%*?&";
        return password.Any(c => specialCharacters.Contains(c));
    }
}