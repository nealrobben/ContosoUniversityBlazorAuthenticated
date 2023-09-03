﻿namespace ContosoUniversityBlazor.Application.Students.Queries.GetStudentsForCourse;

using AutoMapper;
using ContosoUniversityBlazor.Application.Common.Interfaces;
using MediatR;
using WebUI.Shared.Students.Queries.GetStudentsForCourse;
using Microsoft.EntityFrameworkCore;
using global::System.Threading;
using global::System.Threading.Tasks;
using global::System.Collections.Generic;
using global::System.Linq;

public class GetStudentsForCourseQuery : IRequest<StudentsForCourseVM>
{
    public int? ID { get; set; }

    public GetStudentsForCourseQuery(int? id)
    {
        ID = id;
    }
}

public class GetStudentsForCourseQueryHandler : IRequestHandler<GetStudentsForCourseQuery, StudentsForCourseVM>
{
    private readonly ISchoolContext _context;
    private readonly IMapper _mapper;

    public GetStudentsForCourseQueryHandler(ISchoolContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<StudentsForCourseVM> Handle(GetStudentsForCourseQuery request, CancellationToken cancellationToken)
    {
        if (request.ID == null)
            return new StudentsForCourseVM(new List<StudentForCourseVM>());

        var students = await _context.Enrollments
            .Where(x => x.CourseID == request.ID)
            .Include(c => c.Student)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new StudentsForCourseVM(_mapper.Map<List<StudentForCourseVM>>(students));
    }
}