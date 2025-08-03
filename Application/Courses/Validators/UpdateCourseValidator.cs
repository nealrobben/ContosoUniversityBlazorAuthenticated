
using Application.Courses.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Application.Courses.Validators;

public class UpdateCourseValidator
    : AbstractValidator<UpdateCourseCommand>
{
    private readonly ISchoolContext _context;

    public UpdateCourseValidator(ISchoolContext context) : base()
    {
        _context = context;

        RuleFor(p => p.CourseID).NotEmpty();
        RuleFor(p => p.Title).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Credits).NotEmpty().GreaterThan(0);
        RuleFor(p => p.DepartmentID).NotEmpty();

        RuleFor(v => v.Title)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'Title' must be unique.");
    }

    public async Task<bool> BeUniqueTitle(UpdateCourseCommand updateCourse,
        string title, CancellationToken cancellationToken)
    {
        return await _context.Courses
            .AllAsync(x => !x.Title.Equals(title) || x.CourseID == updateCourse.CourseID, cancellationToken);
    }
}
