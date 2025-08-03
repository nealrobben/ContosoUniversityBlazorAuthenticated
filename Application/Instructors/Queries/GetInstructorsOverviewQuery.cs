using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Entities.Projections.Common;
using Domain.Entities.Projections.Instructors;
using Domain.Entities.Projections.Mappers;

namespace Application.Instructors.Queries;

public class GetInstructorsOverviewQuery : IRequest<Overview<InstructorOverview>>
{
    public string SortOrder { get; set; }
    public string SearchString { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }

    public GetInstructorsOverviewQuery(string sortOrder,
        string searchString, int? pageNumber, int? pageSize)
    {
        SortOrder = sortOrder;
        SearchString = searchString;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetInstructorsOverviewQueryHandler : IRequestHandler<GetInstructorsOverviewQuery, Overview<InstructorOverview>>
{
    private readonly ISchoolContext _context;

    private const int _defaultPageSize = 10;

    public GetInstructorsOverviewQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<Overview<InstructorOverview>> Handle(GetInstructorsOverviewQuery request, CancellationToken cancellationToken)
    {
        var instructors = _context.Instructors
            .Search(request.SearchString)
            .Sort(request.SortOrder);

        var totalInstructors = await instructors.CountAsync(cancellationToken);

        var metaData = new MetaData(request.PageNumber ?? 0, totalInstructors,
            request.PageSize ?? _defaultPageSize, request.SortOrder, request.SearchString);

        var items = await instructors
              .Include(i => i.OfficeAssignment)
              .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                    .ThenInclude(i => i.Enrollments)
                        .ThenInclude(i => i.Student)
              .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                    .ThenInclude(i => i.Department)
              .AsNoTracking()
              .Skip(metaData.PageNumber * metaData.PageSize)
              .Take(metaData.PageSize)
              .ToListAsync(cancellationToken);

        return new Overview<InstructorOverview>
        {
            MetaData = metaData,
            Records = items.Select(InstructorProjectionMapper.ToInstructorOverviewProjection).ToList()
        };
    }
}

