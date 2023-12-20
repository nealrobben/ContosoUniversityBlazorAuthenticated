using Application.Instructors.Commands;
using FluentValidation;

namespace Application.Instructors.Validators;

public class UpdateInstructorValidator : AbstractValidator<UpdateInstructorCommand>
{
    public UpdateInstructorValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.HireDate).NotEmpty();
    }
}
