using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extensions;
using System.Linq;
using Application.Common.Interfaces;
using Domain.Entities.Projections.Common;
using Domain.Entities.Projections.Courses;
using Domain.Entities.Projections.Mappers;

namespace Application.Courses.Queries;

public class GetCoursesOverviewQuery : IRequest<Overview<CourseOverview>>
{
    public string SortOrder { get; set; }
    public string SearchString { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }

    public GetCoursesOverviewQuery(string sortOrder,
        string searchString, int? pageNumber, int? pageSize)
    {
        SortOrder = sortOrder;
        SearchString = searchString;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetCoursesOverviewQueryHandler : IRequestHandler<GetCoursesOverviewQuery, Overview<CourseOverview>>
{
    private readonly ISchoolContext _context;

    private const int _defaultPageSize = 10;

    public GetCoursesOverviewQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Overview<CourseOverview>> Handle(GetCoursesOverviewQuery request, CancellationToken cancellationToken)
    {
        var courses = _context.Courses
            .Search(request.SearchString)
            .Sort(request.SortOrder);

        var totalCourses = await courses.CountAsync(cancellationToken);

        var metaData = new MetaData(request.PageNumber ?? 0, totalCourses,
            request.PageSize ?? _defaultPageSize, request.SortOrder, request.SearchString);

        var items = await courses
            .Include(c => c.Department)
            .AsNoTracking()
            .Skip(metaData.PageNumber * metaData.PageSize)
            .Take(metaData.PageSize)
            .ToListAsync(cancellationToken);

        return new Overview<CourseOverview>
        {
            MetaData = metaData,
            Records = items.Select(CourseProjectionMapper.ToCourseOverviewProjection).ToList()
        };
    }
}