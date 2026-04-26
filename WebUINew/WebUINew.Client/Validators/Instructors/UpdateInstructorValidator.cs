using FluentValidation;
using WebUINew.Client.InputModels.Instructors;

namespace WebUINew.Client.Validators.Instructors;

public class UpdateInstructorValidator : AbstractValidator<UpdateInstructorInputModel>
{
    public UpdateInstructorValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.HireDate).NotEmpty();
    }
}