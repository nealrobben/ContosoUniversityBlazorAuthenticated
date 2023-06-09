﻿using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using WebUI.Shared.Departments.Queries.GetDepartmentsLookup;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContosoUniversityBlazor.Application.Departments.Queries.GetDepartmentsLookup;

public class GetDepartmentsLookupQuery : IRequest<DepartmentsLookupVM>
{
}

public class GetDepartmentsLookupQueryHandler : IRequestHandler<GetDepartmentsLookupQuery, DepartmentsLookupVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetDepartmentsLookupQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentsLookupVM> Handle(GetDepartmentsLookupQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Departments
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return new DepartmentsLookupVM(_mapper.Map<List<DepartmentLookupVM>>(list));
    }
}