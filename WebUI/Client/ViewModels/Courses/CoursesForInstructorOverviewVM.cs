using System.Collections.Generic;

namespace WebUI.Client.ViewModels.Courses;

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
