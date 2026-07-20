using System.Collections.Generic;

namespace WebUI.Client.ViewModels.Students;

public class StudentsForCourseVM
{
    public IList<StudentForCourseVM> Students { get; set; }

    public StudentsForCourseVM()
    {
        Students = [];
    }

    public StudentsForCourseVM(IList<StudentForCourseVM> students)
    {
        Students = students;
    }
}