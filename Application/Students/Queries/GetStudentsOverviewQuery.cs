
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Application.Common.Extensions;
using Domain.Entities.Projections.Students;
using Domain.Entities.Projections.Common;
using Domain.Entities.Projections.Mappers;

namespace Application.Students.Queries;

public class GetStudentsOverviewQuery : IRequest<Overview<StudentOverview>>
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

public class GetStudentsOverviewQueryHandler : IRequestHandler<GetStudentsOverviewQuery, Overview<StudentOverview>>
{
    private const int _defaultPageSize = 3;

    private readonly ISchoolContext _context;

    public GetStudentsOverviewQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Overview<StudentOverview>> Handle(GetStudentsOverviewQuery request, CancellationToken cancellationToken)
    {
        var students = _context.Students
            .Search(request.SearchString)
            .Sort(request.SortOrder);

        var totalStudents = await students.CountAsync(cancellationToken);

        var metaData = new MetaData(request.PageNumber ?? 0, totalStudents,
            request.PageSize ?? _defaultPageSize, request.SortOrder, request.SearchString);

        var items = await students.AsNoTracking().Skip(metaData.PageNumber * metaData.PageSize)
            .Take(metaData.PageSize)
            .ToListAsync(cancellationToken);

        return new Overview<StudentOverview>
        {
            MetaData = metaData,
            Records = items.Select(StudentProjectionMapper.ToStudentOverviewProjection).ToList()
        };
    }
}
