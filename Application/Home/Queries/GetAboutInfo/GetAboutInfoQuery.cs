
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Domain.Entities.Projections.Home;

namespace ContosoUniversityBlazor.Application.Home.Queries.GetAboutInfo;

public class GetAboutInfoQuery : IRequest<AboutInfo>
{
}

public class GetAboutInfoQueryHandler : IRequestHandler<GetAboutInfoQuery, AboutInfo>
{
    private readonly ISchoolContext _context;

    public GetAboutInfoQueryHandler(ISchoolContext context)
    {
        _context = context;
    }

    public async Task<AboutInfo> Handle(GetAboutInfoQuery request, CancellationToken cancellationToken)
    {
        var data = from student in _context.Students
                   group student by student.EnrollmentDate into dateGroup
                   select new EnrollmentDateGroup
                   {
                       EnrollmentDate = dateGroup.Key,
                       StudentCount = dateGroup.Count()
                   };

        return new AboutInfo(await data.AsNoTracking().ToListAsync(cancellationToken));
    }
}