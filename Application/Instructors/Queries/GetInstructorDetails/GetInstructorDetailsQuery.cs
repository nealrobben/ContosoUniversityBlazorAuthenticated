
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Domain.Entities.Projections.Instructors;

namespace ContosoUniversityBlazor.Application.Instructors.Queries.GetInstructorDetails;
public class GetInstructorDetailsQuery : IRequest<InstructorDetail>
{
    public int? ID { get; set; }

    public GetInstructorDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, InstructorDetail>
{
    private readonly ISchoolContext _context;

    public GetInstructorDetailsQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<InstructorDetail> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Instructor), request.ID);

        var instructor = await _context.Instructors
            .Include(x => x.OfficeAssignment)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == request.ID, cancellationToken) 
            ?? throw new NotFoundException(nameof(Instructor), request.ID);

        return new InstructorDetail
        {
            InstructorID = instructor.ID,
            LastName = instructor.LastName,
            FirstName = instructor.FirstMidName,
            HireDate = instructor.HireDate,
            OfficeLocation = instructor.OfficeAssignment?.Location ?? string.Empty,
            ProfilePictureName = instructor.ProfilePictureName,
        };
    }
}
