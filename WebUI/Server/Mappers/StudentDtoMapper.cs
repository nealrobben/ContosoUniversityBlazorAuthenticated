using ContosoUniversityBlazor.Domain.Entities;
using Domain.Entities.Projections.Students;
using System.Linq;
using WebUI.Client.Dtos.Students;

namespace WebUI.Server.Mappers;

public static class StudentDtoMapper
{
    public static StudentForCourseDto ToDto(StudentForCourse model)
    {
        return new StudentForCourseDto
        {
            StudentName = model.StudentName,
            StudentGrade = (int)model.StudentGrade
        };
    }

    public static StudentsForCourseDto ToDto(Domain.Entities.Projections.Students.StudentsForCourse model)
    {
        return new StudentsForCourseDto
        {
            Students = model.Students.Select(ToDto).ToList()
        };
    }

    public static StudentDetailEnrollmentDto ToDto(StudentDetailEnrollment model)
    {
        return new StudentDetailEnrollmentDto
        {
            CourseTitle = model.CourseTitle,
            Grade = (int)model.Grade,
        };
    }

    public static StudentDetailDto ToDto(StudentDetail model)
    {
        return new StudentDetailDto
        {
            StudentID = model.StudentID,
            LastName = model.LastName,
            FirstName = model.FirstName,
            EnrollmentDate = model.EnrollmentDate,
            ProfilePictureName = model.ProfilePictureName,
            Enrollments = model.Enrollments.Select(ToDto).ToList()
        };
    }

    public static StudentOverviewDto ToDto(StudentOverview model)
    {
        return new StudentOverviewDto
        {
            StudentID = model.StudentID,
            LastName = model.LastName,
            FirstName = model.FirstName,
            EnrollmentDate = model.EnrollmentDate
        };
    }
}
