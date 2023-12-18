
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Application.Departments.Commands.UpdateDepartment;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Departments.Validators;

public class UpdateDepartmentValidator 
    : AbstractValidator<UpdateDepartmentCommand>
{
    private readonly ISchoolContext _context;

    public UpdateDepartmentValidator(ISchoolContext context) : base()
    {
        _context = context;

        RuleFor(p => p.Name).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Budget).NotEmpty().GreaterThan(0);
        RuleFor(p => p.StartDate).NotEmpty();
        RuleFor(p => p.InstructorID).NotEmpty();

        RuleFor(v => v.Name)
            .MustAsync(BeUniqueName)
                .WithMessage("'Name' must be unique.");
    }

    public async Task<bool> BeUniqueName(UpdateDepartmentCommand updateDepartment, 
        string name, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .AllAsync(x => !x.Name.Equals(name) || x.DepartmentID == updateDepartment.DepartmentID, cancellationToken);
    }
}
