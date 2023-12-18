using FluentValidation;
using WebUI.Client.InputModels.Courses;

namespace WebUI.Client.Validators.Courses;

public class UpdateCourseValidator
    : AbstractValidator<UpdateCourseInputModel>
{
    public UpdateCourseValidator()
    {
        RuleFor(p => p.CourseID).NotEmpty();
        RuleFor(p => p.Title).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Credits).NotEmpty().GreaterThan(0);
        RuleFor(p => p.DepartmentID).NotEmpty();
    }
}
