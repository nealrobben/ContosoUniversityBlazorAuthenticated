using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Commands;

public class UpdateDepartmentCommand : IRequest
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public byte[] RowVersion { get; set; }

    public int InstructorID { get; set; }
}

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly ISchoolContext _context;

    public UpdateDepartmentCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        if (request.DepartmentID == 0)
            throw new NotFoundException(nameof(Department), request.DepartmentID);

        var departmentToUpdate = await _context.Departments
            .Include(i => i.Administrator)
            .FirstOrDefaultAsync(m => m.DepartmentID == request.DepartmentID, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentID);

        departmentToUpdate.Name = request.Name;
        departmentToUpdate.Budget = request.Budget;
        departmentToUpdate.StartDate = request.StartDate;
        departmentToUpdate.InstructorID = request.InstructorID;

        _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = request.RowVersion;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}