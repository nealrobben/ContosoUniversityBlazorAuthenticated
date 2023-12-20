
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Projections.Departments;

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

        return new DepartmentsLookup
        {
            Departments = list.Select(x => new DepartmentLookup
            {
                DepartmentID = x.DepartmentID,
                Name = x.Name
            }).ToList()
        };
    }
}