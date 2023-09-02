namespace WebUI.Shared.Instructors.Validators;

using FluentValidation;
using WebUI.Shared.Instructors.Commands.UpdateInstructor;

public class UpdateInstructorValidator : AbstractValidator<UpdateInstructorCommand>
{
    public UpdateInstructorValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.HireDate).NotEmpty();
    }
}
