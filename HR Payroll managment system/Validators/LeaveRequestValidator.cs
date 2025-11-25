using FluentValidation;
using HR_Payroll_managment_system.Models;

namespace HR_Payroll_managment_system.Validators;

public class LeaveRequestValidator : AbstractValidator<LeaveRequest>
{
    public LeaveRequestValidator()
    {
        RuleFor(l => l.StartDate)
            .NotEmpty().WithMessage("Start Date cannot be empty")
            .GreaterThan(DateTime.Now).WithMessage("Start Date cannot be in the past");
        
        RuleFor(l => l.EndDate)
            .NotEmpty().WithMessage("End Date cannot be empty")
            .GreaterThan(DateTime.Now).WithMessage("End Date cannot be in the past")
            .GreaterThan(l => l.StartDate).WithMessage("End Date cannot be Earlier Than Start Date");
        
        RuleFor(l => l.Reason)
            .NotEmpty().WithMessage("Reason Cannot be empty");
        
    }
}