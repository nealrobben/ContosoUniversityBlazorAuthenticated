using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WebUI.Shared.Courses.Queries.GetCoursesForInstructor;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityBlazor.Application.Courses.Queries.GetCoursesOverview;

public class GetCoursesForInstructorQuery : IRequest<CoursesForInstructorOverviewVM>
{
    public int? ID { get; set; }

    public GetCoursesForInstructorQuery(int? id)
    {
        ID = id;
    }
}

public class GetCoursesForInstructorQueryHandler : IRequestHandler<GetCoursesForInstructorQuery, CoursesForInstructorOverviewVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetCoursesForInstructorQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CoursesForInstructorOverviewVM> Handle(GetCoursesForInstructorQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            return new CoursesForInstructorOverviewVM(new List<CourseForInstructorVM>());

        var courseIdsForInstructor = await _context.CourseAssignments
            .Where(x => x.InstructorID == request.ID)
            .Select(x => x.CourseID)
            .ToListAsync(cancellationToken);

        var courses = await _context.Courses
            .Where(x => courseIdsForInstructor.Contains(x.CourseID))
            .Include(c => c.Department)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new CoursesForInstructorOverviewVM(_mapper.Map<List<CourseForInstructorVM>>(courses));
    }
}