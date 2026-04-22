using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Projections.Departments;
using Domain.Entities.Projections.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Queries;

public class GetDepartmentDetailsQuery : IRequest<DepartmentDetail>
{
    public int? Id { get; set; }

    public GetDepartmentDetailsQuery(int? id)
    {
        Id = id;
    }
}

internal class GetDepartmentDetailsQueryHandler : IRequestHandler<GetDepartmentDetailsQuery, DepartmentDetail>
{
    private readonly ISchoolContext _context;

    public GetDepartmentDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<DepartmentDetail> Handle(GetDepartmentDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == null)
            throw new NotFoundException(nameof(Department), request.Id);

        var department = await _context.Departments
            .Include(i => i.Administrator)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.DepartmentID == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.Id);

        return DepartmentProjectionMapper.ToDepartmentDetailProjection(department);
    }
}
