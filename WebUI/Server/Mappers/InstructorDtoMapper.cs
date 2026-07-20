using System.Linq;
using Domain.Entities.Projections.Instructors;
using WebUI.Client.Dtos.Instructors;

namespace WebUI.Server.Mappers;

public static class InstructorDtoMapper
{
    public static InstructorLookupDto ToDto(InstructorLookup model)
    {
        return new InstructorLookupDto(model.ID, model.FullName);
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
        return new InstructorDetailDto(model.InstructorID, model.LastName, model.FirstName, model.HireDate, model.OfficeLocation, model.ProfilePictureName);
    }

    public static CourseAssignmentDto ToDto(CourseAssignment model)
    {
        return new CourseAssignmentDto(model.CourseID, model.CourseTitle);
    }

    public static InstructorOverviewDto ToDto(InstructorOverview model)
    {
        return new InstructorOverviewDto(model.InstructorID, model.LastName, model.FirstName, model.HireDate, model.OfficeLocation, model.CourseAssignments.Select(ToDto).ToList());
    }
}
