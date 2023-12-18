
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Projections.Courses;

namespace ContosoUniversityBlazor.Application.Courses.Queries.GetCourseDetails;

public class GetCourseDetailsQuery : IRequest<CourseDetail>
{
    public int? ID { get; set; }

    public GetCourseDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, CourseDetail>
{
    private readonly ISchoolContext _context;

    public GetCourseDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<CourseDetail> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Course), request.ID);

        var course = await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CourseID == request.ID, cancellationToken)
            ?? throw new NotFoundException(nameof(Course), request.ID);

        return new CourseDetail
        {
            CourseID = course.CourseID,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentID = course.DepartmentID
        };
    }
}