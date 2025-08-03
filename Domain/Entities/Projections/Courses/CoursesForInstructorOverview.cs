using System.Collections.Generic;

namespace Domain.Entities.Projections.Courses;

public  class CoursesForInstructorOverview
{
    public IList<CourseForInstructor> Courses { get; set; }

    public CoursesForInstructorOverview()
    {
        Courses = new List<CourseForInstructor>();
    }

    public CoursesForInstructorOverview(IList<CourseForInstructor> courses)
    {
        Courses = courses;
    }
}
