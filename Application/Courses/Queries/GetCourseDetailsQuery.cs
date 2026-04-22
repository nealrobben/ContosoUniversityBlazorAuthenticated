using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Projections.Courses;
using Domain.Entities.Projections.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Courses.Queries;

public class GetCourseDetailsQuery : IRequest<CourseDetail>
{
    public int? Id { get; set; }

    public GetCourseDetailsQuery(int? id)
    {
        Id = id;
    }
}

internal class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, CourseDetail>
{
    private readonly ISchoolContext _context;

    public GetCourseDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<CourseDetail> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.Id == null)
            throw new NotFoundException(nameof(Course), request.Id);

        var course = await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CourseID == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Course), request.Id);

        return CourseProjectionMapper.ToCourseDetailProjection(course);
    }
}