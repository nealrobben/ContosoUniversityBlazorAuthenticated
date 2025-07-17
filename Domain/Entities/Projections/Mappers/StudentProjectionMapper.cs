
using ContosoUniversityBlazor.Domain.Entities;
using Domain.Entities.Projections.Students;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.Projections.Mappers;

public static class StudentProjectionMapper
{
    public static StudentDetailEnrollment ToStudentDetailEnrollmentProjection(Enrollment enrollment)
    {
        return new StudentDetailEnrollment
        {
            CourseTitle = enrollment.Course.Title,
            Grade = enrollment.Grade,
        };
    }

    public static StudentDetail ToStudentDetailProjection(Student student)
    {
        return new StudentDetail
        {
            StudentID = student.ID,
            LastName = student.LastName,
            FirstName = student.FirstMidName,
            EnrollmentDate = student.EnrollmentDate,
            ProfilePictureName = student.ProfilePictureName,
            Enrollments = student.Enrollments.Select(ToStudentDetailEnrollmentProjection).ToList()
        };
    }

    public static StudentOverview ToStudentOverviewProjection(Student student)
    {
        return new StudentOverview
        {
            StudentID = student.ID,
            LastName = student.LastName,
            FirstName = student.FirstMidName,
            EnrollmentDate = student.EnrollmentDate,
        };
    }

    public static StudentForCourse ToStudentForCourseProjection(Enrollment enrollment)
    {
        return new StudentForCourse
        {
            StudentName = enrollment.Student.FullName,
            StudentGrade = enrollment.Grade
        };
    }

    public static StudentsForCourse ToStudentsForCourseProjection(IEnumerable<Enrollment> enrollments)
    {
        return new StudentsForCourse
        {
            Students = enrollments.Select(ToStudentForCourseProjection).ToList()
        };
    }
}
