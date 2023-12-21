
using ContosoUniversityBlazor.Domain.Entities;
using Domain.Entities.Projections.Instructors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Projections.Mappers;

public static class InstructorProjectionMapper
{
    public static InstructorDetail ToInstructorDetailProjection(Instructor instructor)
    {
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

    public static Instructors.CourseAssignment ToCourseAssignmentProjection(ContosoUniversityBlazor.Domain.Entities.CourseAssignment courseAssignment)
    {
        return new Instructors.CourseAssignment
        {
            CourseID = courseAssignment.CourseID,
            CourseTitle = courseAssignment.Course.Title
        };
    }

    public static InstructorLookup ToInstructorLookupProjection(Instructor instructor)
    {
        return new InstructorLookup
        {
            ID = instructor.ID,
            FullName = instructor.FullName
        };
    }

    public static InstructorsLookup ToInstructorsLookupProjection(IEnumerable<Instructor> instructors)
    {
        return new InstructorsLookup
        {
            Instructors = instructors.Select(ToInstructorLookupProjection).ToList()
        };
    }

    public static InstructorOverview ToInstructorOverviewProjection(Instructor instructor)
    {
        return new InstructorOverview
        {
            InstructorID = instructor.ID,
            LastName = instructor.LastName,
            FirstName = instructor.FirstMidName,
            HireDate = instructor.HireDate,
            OfficeLocation = instructor.OfficeAssignment?.Location ?? string.Empty,
            CourseAssignments = instructor.CourseAssignments.Select(ToCourseAssignmentProjection).ToList()
        };
    }
}
