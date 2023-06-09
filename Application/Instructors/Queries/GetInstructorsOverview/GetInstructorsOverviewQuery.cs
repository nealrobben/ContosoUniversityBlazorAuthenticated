using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WebUI.Shared.Common;
using WebUI.Shared.Instructors.Queries.GetInstructorsOverview;
using Application.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContosoUniversityCQRS.Application.Instructors.Queries.GetInstructorsOverview;

public class GetInstructorsOverviewQuery : IRequest<OverviewVM<InstructorVM>>
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

public class GetInstructorsOverviewQueryHandler : IRequestHandler<GetInstructorsOverviewQuery, OverviewVM<InstructorVM>>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    private const int _defaultPageSize = 10;

    public GetInstructorsOverviewQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OverviewVM<InstructorVM>> Handle(GetInstructorsOverviewQuery request, CancellationToken cancellationToken)
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
              .Skip((metaData.PageNumber) * metaData.PageSize)
              .Take(metaData.PageSize)
              .ToListAsync(cancellationToken);

        return new OverviewVM<InstructorVM>(_mapper.Map<List<InstructorVM>>(items), metaData);
    }
}

