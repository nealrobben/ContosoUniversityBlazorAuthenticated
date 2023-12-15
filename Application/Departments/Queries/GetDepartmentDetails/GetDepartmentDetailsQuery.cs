namespace ContosoUniversityBlazor.Application.Departments.Queries.GetDepartmentDetails;

using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Exceptions;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using ContosoUniversityBlazor.Domain.Entities;
using MediatR;
using WebUI.Shared.Departments.Queries.GetDepartmentDetails;
using Microsoft.EntityFrameworkCore;
using global::System.Threading;
using global::System.Threading.Tasks;

public class GetDepartmentDetailsQuery : IRequest<DepartmentDetailVM>
{
    public int? ID { get; set; }

    public GetDepartmentDetailsQuery(int? id)
    {
        ID = id;
    }
}

public class GetDepartmentDetailsQueryHandler : IRequestHandler<GetDepartmentDetailsQuery, DepartmentDetailVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetDepartmentDetailsQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentDetailVM> Handle(GetDepartmentDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            throw new NotFoundException(nameof(Department), request.ID);

        var department = await _context.Departments
            .Include(i => i.Administrator)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.DepartmentID == request.ID, cancellationToken) 
            ?? throw new NotFoundException(nameof(Department), request.ID);

        return _mapper.Map<DepartmentDetailVM>(department);
    }
}
