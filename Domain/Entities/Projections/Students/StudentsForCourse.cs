using System.Collections.Generic;

namespace Domain.Entities.Projections.Students;

public class StudentsForCourse
{
    public IList<StudentForCourse> Students { get; set; }

    public StudentsForCourse()
    {
        Students = new List<StudentForCourse>();
    }

    public StudentsForCourse(IList<StudentForCourse> students)
    {
        Students = students;
    }
}
