using FluentValidation;
using WebUINew.Client.InputModels.Departments;

namespace WebUINew.Client.Validators.Departments;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentInputModel>
{
    public CreateDepartmentValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Budget).NotEmpty().GreaterThan(0);
        RuleFor(p => p.StartDate).NotEmpty();
        RuleFor(p => p.InstructorID).NotEmpty();
    }
}