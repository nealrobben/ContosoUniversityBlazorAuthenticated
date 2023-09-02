namespace WebUI.Shared.Courses.Queries.GetCoursesForInstructor;

using System.Collections.Generic;

public class CoursesForInstructorOverviewVM
{
    public IList<CourseForInstructorVM> Courses { get; set; }

    public CoursesForInstructorOverviewVM()
    {
        Courses = new List<CourseForInstructorVM>();
    }

    public CoursesForInstructorOverviewVM(IList<CourseForInstructorVM> courses)
    {
        Courses = courses;
    }
}
