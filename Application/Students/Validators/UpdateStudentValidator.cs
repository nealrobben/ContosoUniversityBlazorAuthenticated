using Application.Students.Commands;
using FluentValidation;

namespace Application.Students.Validators;

public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.EnrollmentDate).NotEmpty();
    }
}
