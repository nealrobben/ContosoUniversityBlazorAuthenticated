using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Interfaces;
using Domain.Entities.Projections.Students;
using Domain.Entities.Projections.Mappers;

namespace Application.Students.Queries;

public class GetStudentsForCourseQuery : IRequest<StudentsForCourse>
{
    public int? ID { get; set; }

    public GetStudentsForCourseQuery(int? id)
    {
        ID = id;
    }
}

public class GetStudentsForCourseQueryHandler : IRequestHandler<GetStudentsForCourseQuery, StudentsForCourse>
{
    private readonly ISchoolContext _context;

    public GetStudentsForCourseQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<StudentsForCourse> Handle(GetStudentsForCourseQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            return new StudentsForCourse(new List<StudentForCourse>());

        var students = await _context.Enrollments
            .Where(x => x.CourseID == request.ID)
            .Include(c => c.Student)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return StudentProjectionMapper.ToStudentsForCourseProjection(students);
    }
}