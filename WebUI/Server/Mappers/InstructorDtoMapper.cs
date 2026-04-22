using System.Linq;
using Domain.Entities.Projections.Instructors;
using WebUI.Client.Dtos.Instructors;

namespace WebUI.Server.Mappers;

public static class InstructorDtoMapper
{
    public static InstructorLookupDto ToDto(InstructorLookup model)
    {
        return new InstructorLookupDto
        {
            Id = model.ID,
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
            InstructorId = model.InstructorID,
            LastName = model.LastName,
            FirstName = model.FirstName,
            HireDate = model.HireDate,
            OfficeLocation = model.OfficeLocation,
            ProfilePictureName = model.ProfilePictureName,
        };
    }

    public static CourseAssignmentDto ToDto(CourseAssignment model)
    {
        return new CourseAssignmentDto
        {
            CourseId = model.CourseID,
            CourseTitle = model.CourseTitle
        };
    }

    public static InstructorOverviewDto ToDto(InstructorOverview model)
    {
        return new InstructorOverviewDto
        {
            InstructorId = model.InstructorID,
            LastName = model.LastName,
            FirstName = model.FirstName,
            HireDate = model.HireDate,
            OfficeLocation = model.OfficeLocation,
            CourseAssignments = model.CourseAssignments.Select(ToDto).ToList()
        };
    }
}
