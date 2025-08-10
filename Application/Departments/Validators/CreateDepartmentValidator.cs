using Application.Departments.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Application.Departments.Validators;

public class CreateDepartmentValidator 
    : AbstractValidator<CreateDepartmentCommand>
{
    private readonly ISchoolContext _context;

    public CreateDepartmentValidator(ISchoolContext context)
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

    public async Task<bool> BeUniqueName(CreateDepartmentCommand createDepartment, 
        string name, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .AllAsync(x => !x.Name.Equals(name), cancellationToken);
    }
}
