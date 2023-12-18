using ContosoUniversityBlazor.Application.Instructors.Commands.CreateInstructor;
using FluentValidation;

namespace Application.Instructors.Validators;

public class CreateInstructorValidator : AbstractValidator<CreateInstructorCommand>
{
    public CreateInstructorValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.HireDate).NotEmpty();
    }
}
