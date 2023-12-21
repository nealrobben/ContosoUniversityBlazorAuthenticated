using System.Linq;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.ViewModels.Instructors;

namespace WebUI.Client.Mappers;

public static class InstructorViewModelMapper
{
    public static InstructorDetailVM ToViewModel(InstructorDetailDto dto)
    {
        return new InstructorDetailVM
        {
            InstructorID = dto.InstructorID,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            HireDate = dto.HireDate,
            OfficeLocation = dto.OfficeLocation,
            ProfilePictureName = dto.ProfilePictureName,
        };
    }

    public static CourseAssignmentVM ToViewModel(CourseAssignmentDto dto)
    {
        return new CourseAssignmentVM
        {
            CourseID = dto.CourseID,
            CourseTitle = dto.CourseTitle
        };
    }

    public static InstructorOverviewVM ToViewModel(InstructorOverviewDto dto)
    {
        return new InstructorOverviewVM
        {
            InstructorID = dto.InstructorID,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            HireDate = dto.HireDate,
            OfficeLocation = dto.OfficeLocation,
            CourseAssignments = dto.CourseAssignments.Select(ToViewModel).ToList()
        };
    }

    public static InstructorLookupVM ToViewModel(InstructorLookupDto dto)
    {
        return new InstructorLookupVM
        {
            ID = dto.ID,
            FullName = dto.FullName
        };
    }

    public static InstructorsLookupVM ToViewModel(InstructorsLookupDto dto)
    {
        return new InstructorsLookupVM
        {
            Instructors = dto.Instructors.Select(ToViewModel).ToList()
        };
    }
}
