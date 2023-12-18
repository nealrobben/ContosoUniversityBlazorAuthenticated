using FluentValidation;
using WebUI.Client.InputModels.Departments;

namespace WebUI.Client.Validators.Departments;

public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentInputModel>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Budget).NotEmpty().GreaterThan(0);
        RuleFor(p => p.StartDate).NotEmpty();
        RuleFor(p => p.InstructorID).NotEmpty();
    }
}
