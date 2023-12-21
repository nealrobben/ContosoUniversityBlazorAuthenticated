using System.Linq;
using WebUI.Client.Dtos.Students;
using WebUI.Client.Enums;
using WebUI.Client.ViewModels.Students;

namespace WebUI.Client.Mappers;

public static class StudentViewModelMapper
{
    public static StudentDetailEnrollmentVM ToViewModel(StudentDetailEnrollmentDto dto)
    {
        return new StudentDetailEnrollmentVM
        {
            CourseTitle = dto.CourseTitle,
            Grade = (Grade?)dto.Grade,
        };
    }

    public static StudentDetailVM ToViewModel(StudentDetailDto dto)
    {
        return new StudentDetailVM
        {
            StudentID = dto.StudentID,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            EnrollmentDate = dto.EnrollmentDate,
            ProfilePictureName = dto.ProfilePictureName,
            Enrollments = dto.Enrollments.Select(ToViewModel).ToList()
        };
    }

    public static StudentOverviewVM ToViewModel(StudentOverviewDto dto)
    {
        return new StudentOverviewVM
        {
            StudentID = dto.StudentID,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            EnrollmentDate = dto.EnrollmentDate
        };
    }

    public static StudentForCourseVM ToViewModel(StudentForCourseDto dto)
    {
        return new StudentForCourseVM
        {
            StudentName = dto.StudentName,
            StudentGrade = (Grade?)dto.StudentGrade
        };
    }

    public static StudentsForCourseVM ToViewModel(StudentsForCourseDto dto)
    {
        return new StudentsForCourseVM
        {
            Students = dto.Students.Select(ToViewModel).ToList()
        };
    }
}
