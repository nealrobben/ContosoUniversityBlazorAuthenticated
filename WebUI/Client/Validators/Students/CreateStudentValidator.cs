using FluentValidation;
using WebUI.Client.InputModels.Students;

namespace WebUI.Client.Validators.Students;

public class CreateStudentValidator : AbstractValidator<CreateStudentInputModel>
{
    public CreateStudentValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.EnrollmentDate).NotEmpty();
    }
}
