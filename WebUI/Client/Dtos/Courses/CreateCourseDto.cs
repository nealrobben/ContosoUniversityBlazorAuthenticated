﻿namespace WebUI.Client.Dtos.Courses;

public class CreateCourseDto
{
    public int CourseID { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentID { get; set; }
}
