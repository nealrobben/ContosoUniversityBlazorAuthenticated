using WebUINew.Client.Dtos.Instructors;
using WebUINew.Client.ViewModels.Instructors;

namespace WebUINew.Client.Mappers;

public static class InstructorViewModelMapper
{
    public static InstructorDetailVM ToViewModel(InstructorDetailDto dto)
    {
        return new InstructorDetailVM
        {
            InstructorId = dto.InstructorId,
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
            CourseId = dto.CourseId,
            CourseTitle = dto.CourseTitle
        };
    }

    public static InstructorOverviewVM ToViewModel(InstructorOverviewDto dto)
    {
        return new InstructorOverviewVM
        {
            InstructorId = dto.InstructorId,
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
            Id = dto.Id,
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