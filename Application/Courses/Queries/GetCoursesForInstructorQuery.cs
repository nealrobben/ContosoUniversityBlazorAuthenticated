
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Domain.Entities.Projections.Courses;
using Domain.Entities.Projections.Mappers;

namespace Application.Courses.Queries;

public class GetCoursesForInstructorQuery : IRequest<CoursesForInstructorOverview>
{
    public int? ID { get; set; }

    public GetCoursesForInstructorQuery(int? id)
    {
        ID = id;
    }
}

public class GetCoursesForInstructorQueryHandler : IRequestHandler<GetCoursesForInstructorQuery, CoursesForInstructorOverview>
{
    private readonly ISchoolContext _context;

    public GetCoursesForInstructorQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<CoursesForInstructorOverview> Handle(GetCoursesForInstructorQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            return new CoursesForInstructorOverview(new List<CourseForInstructor>());

        var courseIdsForInstructor = await _context.CourseAssignments
            .Where(x => x.InstructorID == request.ID)
            .Select(x => x.CourseID)
            .ToListAsync(cancellationToken);

        var courses = await _context.Courses
            .Where(x => courseIdsForInstructor.Contains(x.CourseID))
            .Include(c => c.Department)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return CourseProjectionMapper.ToCoursesForInstructorProjection(courses);
    }
}