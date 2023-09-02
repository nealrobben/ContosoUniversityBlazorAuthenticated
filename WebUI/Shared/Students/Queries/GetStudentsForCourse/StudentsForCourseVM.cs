namespace WebUI.Shared.Students.Queries.GetStudentsForCourse;

using System.Collections.Generic;

public class StudentsForCourseVM
{
    public IList<StudentForCourseVM> Students { get; set; }

    public StudentsForCourseVM()
    {
        Students = new List<StudentForCourseVM>();
    }

    public StudentsForCourseVM(IList<StudentForCourseVM> students)
    {
        Students = students;
    }
}
