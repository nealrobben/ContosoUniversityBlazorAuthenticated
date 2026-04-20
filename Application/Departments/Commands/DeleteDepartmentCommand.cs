using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Commands;

public class DeleteDepartmentCommand : IRequest
{
    public int Id { get; set; }

    public DeleteDepartmentCommand(int id)
    {
        Id = id;
    }
}

internal class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly ISchoolContext _context;

    public DeleteDepartmentCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.SingleOrDefaultAsync(x => x.DepartmentID == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.Id);

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
