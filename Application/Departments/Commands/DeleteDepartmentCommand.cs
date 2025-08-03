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
    public int ID { get; set; }

    public DeleteDepartmentCommand(int id)
    {
        ID = id;
    }
}

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly ISchoolContext _context;

    public DeleteDepartmentCommandHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.SingleOrDefaultAsync(x => x.DepartmentID == request.ID, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.ID);

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
