
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Projections.Departments;

namespace Application.Departments.Queries;

public class GetDepartmentDetailsQuery : IRequest<DepartmentDetail>
{
    public int? ID { get; set; }

    public GetDepartmentDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetDepartmentDetailsQueryHandler : IRequestHandler<GetDepartmentDetailsQuery, DepartmentDetail>
{
    private readonly ISchoolContext _context;

    public GetDepartmentDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<DepartmentDetail> Handle(GetDepartmentDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Department), request.ID);

        var department = await _context.Departments
            .Include(i => i.Administrator)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.DepartmentID == request.ID, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.ID);

        return new DepartmentDetail
        {
            DepartmentID = department.DepartmentID,
            Name = department.Name,
            Budget = department.Budget,
            StartDate = department.StartDate,
            AdministratorName = department.Administrator?.FullName ?? string.Empty,
            InstructorID = department.InstructorID,
            RowVersion = department.RowVersion
        };
    }
}
