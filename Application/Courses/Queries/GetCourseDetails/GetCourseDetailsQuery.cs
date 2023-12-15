namespace ContosoUniversityBlazor.Application.Courses.Queries.GetCourseDetails;

using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using WebUI.Shared.Courses.Queries.GetCourseDetails;
using Microsoft.EntityFrameworkCore;
using global::System.Threading;
using global::System.Threading.Tasks;

public class GetCourseDetailsQuery : IRequest<CourseDetailVM>
{
    public int? ID { get; set; }

    public GetCourseDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, CourseDetailVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetCourseDetailsQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CourseDetailVM> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Course), request.ID);

        var course = await _context.Courses
            .Include(c => c.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.CourseID == request.ID, cancellationToken)
            ?? throw new NotFoundException(nameof(Course), request.ID);

        return _mapper.Map<CourseDetailVM>(course);
    }
}