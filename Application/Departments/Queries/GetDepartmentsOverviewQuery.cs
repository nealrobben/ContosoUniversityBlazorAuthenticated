
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Extensions;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Domain.Entities.Projections.Departments;
using Domain.Entities.Projections.Common;

namespace Application.Departments.Queries;

public class GetDepartmentsOverviewQuery : IRequest<Overview<DepartmentOverview>>
{
    public string SortOrder { get; set; }
    public string SearchString { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }

    public GetDepartmentsOverviewQuery(string sortOrder,
        string searchString, int? pageNumber, int? pageSize)
    {
        SortOrder = sortOrder;
        SearchString = searchString;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetDepartmentsOverviewQueryHandler : IRequestHandler<GetDepartmentsOverviewQuery, Overview<DepartmentOverview>>
{
    private readonly ISchoolContext _context;

    private const int _defaultPageSize = 10;

    public GetDepartmentsOverviewQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Overview<DepartmentOverview>> Handle(GetDepartmentsOverviewQuery request, CancellationToken cancellationToken)
    {
        var departments = _context.Departments
            .Search(request.SearchString)
            .Sort(request.SortOrder);

        var totalDepartments = await departments.CountAsync(cancellationToken);

        var metaData = new MetaData(request.PageNumber ?? 0, totalDepartments,
            request.PageSize ?? _defaultPageSize, request.SortOrder, request.SearchString);

        var items = await departments
            .Include(d => d.Administrator)
            .AsNoTracking()
            .Skip(metaData.PageNumber * metaData.PageSize)
            .Take(metaData.PageSize)
            .ToListAsync(cancellationToken);

        return new Overview<DepartmentOverview>
        {
            MetaData = metaData,
            Records = items.Select(x => new DepartmentOverview
            {
                DepartmentID = x.DepartmentID,
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                AdministratorName = x.Administrator?.FullName ?? string.Empty
            }).ToList()
        };
    }
}