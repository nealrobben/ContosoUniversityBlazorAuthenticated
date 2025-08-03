using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Projections.Courses;
using Domain.Entities.Projections.Mappers;

namespace Application.Courses.Queries;

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

        return CourseProjectionMapper.ToCourseDetailProjection(course);
    }
}