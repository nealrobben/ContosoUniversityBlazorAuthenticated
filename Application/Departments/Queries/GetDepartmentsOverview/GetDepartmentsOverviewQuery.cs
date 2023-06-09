using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WebUI.Shared.Common;
using WebUI.Shared.Departments.Queries.GetDepartmentsOverview;
using Application.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContosoUniversityBlazor.Application.Departments.Queries.GetDepartmentsOverview;

public class GetDepartmentsOverviewQuery : IRequest<OverviewVM<DepartmentVM>>
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

public class GetDepartmentsOverviewQueryHandler : IRequestHandler<GetDepartmentsOverviewQuery, OverviewVM<DepartmentVM>>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    private const int _defaultPageSize = 10;

    public GetDepartmentsOverviewQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OverviewVM<DepartmentVM>> Handle(GetDepartmentsOverviewQuery request, CancellationToken cancellationToken)
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
            .Skip((metaData.PageNumber) * metaData.PageSize)
            .Take(metaData.PageSize)
            .ToListAsync(cancellationToken);

        return new OverviewVM<DepartmentVM>(_mapper.Map<List<DepartmentVM>>(items), metaData);
    }
}