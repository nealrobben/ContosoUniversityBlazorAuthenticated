using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.Projections.Courses;
using Domain.Entities.Projections.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries;

public class GetCoursesForInstructorQuery : IRequest<CoursesForInstructorOverview>
{
    public int? Id { get; set; }

    public GetCoursesForInstructorQuery(int? id)
    {
        Id = id;
    }
}

internal class GetCoursesForInstructorQueryHandler : IRequestHandler<GetCoursesForInstructorQuery, CoursesForInstructorOverview>
{
    private readonly ISchoolContext _context;

    public GetCoursesForInstructorQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<CoursesForInstructorOverview> Handle(GetCoursesForInstructorQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == null)
            return new CoursesForInstructorOverview(new List<CourseForInstructor>());

        var courseIdsForInstructor = await _context.CourseAssignments
            .Where(x => x.InstructorID == request.Id)
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