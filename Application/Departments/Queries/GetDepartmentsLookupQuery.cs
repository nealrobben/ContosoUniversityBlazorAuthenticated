
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Projections.Departments;
using Domain.Entities.Projections.Mappers;

namespace Application.Departments.Queries;

public class GetDepartmentsLookupQuery : IRequest<DepartmentsLookup>
{
}

public class GetDepartmentsLookupQueryHandler : IRequestHandler<GetDepartmentsLookupQuery, DepartmentsLookup>
{
    private readonly ISchoolContext _context;

    public GetDepartmentsLookupQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<DepartmentsLookup> Handle(GetDepartmentsLookupQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Departments
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return DepartmentProjectionMapper.ToDepartmentsLookupProjection(list);
    }
}