namespace ContosoUniversityBlazor.Application.Departments.Commands.DeleteDepartment;

using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

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
        var department = await _context.Departments.FindAsync(request.ID, cancellationToken);

        if (department == null)
            throw new NotFoundException(nameof(Department), request.ID);

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
