using ContosoUniversityBlazor.Domain.Entities;
using Domain.Entities.Projections.Instructors;
using System.Linq;
using WebUI.Client.Dtos.Instructors;

namespace WebUI.Server.Mappers;

public static class InstructorDtoMapper
{
    public static InstructorLookupDto ToDto(InstructorLookup model)
    {
        return new InstructorLookupDto
        {
            ID = model.ID,
            FullName = model.FullName
        };
    }

    public static InstructorsLookupDto ToDto(InstructorsLookup model)
    {
        return new InstructorsLookupDto
        {
            Instructors = model.Instructors.Select(ToDto).ToList()
        };
    }

    public static InstructorDetailDto ToDto(InstructorDetail model)
    {
        return new InstructorDetailDto
        {
            InstructorID = model.InstructorID,
            LastName = model.LastName,
            FirstName = model.FirstName,
            HireDate = model.HireDate,
            OfficeLocation = model.OfficeLocation,
            ProfilePictureName = model.ProfilePictureName,
        };
    }

    public static CourseAssignmentDto ToDto(Domain.Entities.Projections.Instructors.CourseAssignment model)
    {
        return new CourseAssignmentDto
        {
            CourseID = model.CourseID,
            CourseTitle = model.CourseTitle
        };
    }

    public static InstructorOverviewDto ToDto(InstructorOverview model)
    {
        return new InstructorOverviewDto
        {
            InstructorID = model.InstructorID,
            LastName = model.LastName,
            FirstName = model.FirstName,
            HireDate = model.HireDate,
            OfficeLocation = model.OfficeLocation,
            CourseAssignments = model.CourseAssignments.Select(ToDto).ToList()
        };
    }
}
