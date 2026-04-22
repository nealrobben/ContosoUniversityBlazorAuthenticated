using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Departments.Commands;

public class CreateDepartmentCommand : IRequest<int>
{
    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public int InstructorId { get; set; }
}

internal class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, int>
{
    private readonly ISchoolContext _context;

    public CreateDepartmentCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var newDepartment = new Department
        {
            Name = request.Name,
            Budget = request.Budget,
            StartDate = request.StartDate,
            InstructorID = request.InstructorId
        };

        _context.Departments.Add(newDepartment);

        await _context.SaveChangesAsync(cancellationToken);

        return newDepartment.DepartmentID;
    }
}
