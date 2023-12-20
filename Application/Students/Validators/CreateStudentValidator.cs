using Application.Students.Commands;
using FluentValidation;

namespace Application.Students.Validators;

public class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.EnrollmentDate).NotEmpty();
    }
}
