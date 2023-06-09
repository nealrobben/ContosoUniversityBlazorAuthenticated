using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WebUI.Shared.Common;
using WebUI.Shared.Students.Queries.GetStudentsOverview;
using Application.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContosoUniversityBlazor.Application.Students.Queries.GetStudentsOverview;

public class GetStudentsOverviewQuery : IRequest<OverviewVM<StudentOverviewVM>>
{
    public string SortOrder { get; set; }
    public string SearchString { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }

    public GetStudentsOverviewQuery(string sortOrder,
        string searchString, int? pageNumber, int? pageSize)
    {
        SortOrder = sortOrder;
        SearchString = searchString;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetStudentsOverviewQueryHandler : IRequestHandler<GetStudentsOverviewQuery, OverviewVM<StudentOverviewVM>>
{
    private const int _defaultPageSize = 3;

    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetStudentsOverviewQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OverviewVM<StudentOverviewVM>> Handle(GetStudentsOverviewQuery request, CancellationToken cancellationToken)
    {
        var students = _context.Students
            .Search(request.SearchString)
            .Sort(request.SortOrder);

        var totalStudents = await students.CountAsync(cancellationToken);

        var metaData = new MetaData(request.PageNumber ?? 0, totalStudents,
            request.PageSize ?? _defaultPageSize, request.SortOrder, request.SearchString);

        var items = await students.AsNoTracking().Skip((metaData.PageNumber) * metaData.PageSize)
            .Take(metaData.PageSize)
            .ToListAsync(cancellationToken);

        return new OverviewVM<StudentOverviewVM>(_mapper.Map<List<StudentOverviewVM>>(items), metaData);
    }
}
