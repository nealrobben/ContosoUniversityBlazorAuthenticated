﻿
using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using WebUI.Shared.Common;
using WebUI.Shared.Courses.Queries.GetCoursesOverview;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extensions;
using System.Linq;

namespace ContosoUniversityBlazor.Application.Courses.Queries.GetCoursesOverview;

public class GetCoursesOverviewQuery : IRequest<OverviewVM<CourseVM>>
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

public class GetCoursesOverviewQueryHandler : IRequestHandler<GetCoursesOverviewQuery, OverviewVM<CourseVM>>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    private const int _defaultPageSize = 10;

    public GetCoursesOverviewQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OverviewVM<CourseVM>> Handle(GetCoursesOverviewQuery request, CancellationToken cancellationToken)
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
            .Skip((metaData.PageNumber) * metaData.PageSize)
            .Take(metaData.PageSize)
            .ToListAsync(cancellationToken);

        return new OverviewVM<CourseVM>(_mapper.Map<List<CourseVM>>(items), metaData);
    }
}