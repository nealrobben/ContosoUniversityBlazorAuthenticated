using WebUINew.Client.Dtos.Students;
using WebUINew.Client.Enums;
using WebUINew.Client.ViewModels.Students;

namespace WebUINew.Client.Mappers;

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
            StudentId = dto.StudentID,
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
            StudentId = dto.StudentId,
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