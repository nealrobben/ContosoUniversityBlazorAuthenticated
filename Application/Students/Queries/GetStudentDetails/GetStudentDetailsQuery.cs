namespace ContosoUniversityBlazor.Application.Students.Queries.GetStudentDetails;

using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using WebUI.Shared.Students.Queries.GetStudentDetails;
using Microsoft.EntityFrameworkCore;
using global::System.Threading.Tasks;
using global::System.Threading;

public class GetStudentDetailsQuery : IRequest<StudentDetailsVM>
{
    public int? ID { get; set; }

    public GetStudentDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetStudentDetailsQueryHandler : IRequestHandler<GetStudentDetailsQuery, StudentDetailsVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetStudentDetailsQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<StudentDetailsVM> Handle(GetStudentDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Student), request.ID);

        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == request.ID, cancellationToken);

        if (student == null)
            throw new NotFoundException(nameof(Student), request.ID);

        return _mapper.Map<StudentDetailsVM>(student);
    }
}