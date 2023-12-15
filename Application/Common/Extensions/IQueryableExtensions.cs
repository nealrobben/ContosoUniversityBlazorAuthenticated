namespace Application.Common.Extensions;

using ContosoUniversityBlazor.Domain.Entities;
using System;
using System.Linq;

public static class IQueryableExtensions
{
    public static IQueryable<Student> Search(this IQueryable<Student> value, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return value;

        return value.Where(s => s.LastName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                   || s.FirstMidName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<Department> Search(this IQueryable<Department> value, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return value;

        return value.Where(s => s.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<Instructor> Search(this IQueryable<Instructor> value, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return value;

        return value.Where(s => s.LastName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                   || s.FirstMidName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<Course> Search(this IQueryable<Course> value, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return value;

        return value.Where(s => s.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<Student> Sort(this IQueryable<Student> value, string sortString)
    {
        return (sortString?.ToLower() ?? "") switch
        {
            "lastname_asc" => value.OrderBy(s => s.LastName),
            "lastname_desc" => value.OrderByDescending(s => s.LastName),
            "firstname_asc" => value.OrderBy(s => s.FirstMidName),
            "firstname_desc" => value.OrderByDescending(s => s.FirstMidName),
            "enrollmentdate_asc" => value.OrderBy(s => s.EnrollmentDate),
            "enrollmentdate_desc" => value.OrderByDescending(s => s.EnrollmentDate),
            _ => value.OrderBy(s => s.LastName),
        };
    }

    public static IQueryable<Department> Sort(this IQueryable<Department> value, string sortString)
    {
        return (sortString?.ToLower() ?? "") switch
        {
            "name_asc" => value.OrderBy(s => s.Name),
            "name_desc" => value.OrderByDescending(s => s.Name),
            _ => value.OrderBy(s => s.Name),
        };
    }

    public static IQueryable<Instructor> Sort(this IQueryable<Instructor> value, string sortString)
    {
        return (sortString?.ToLower() ?? "") switch
        {
            "lastname_asc" => value.OrderBy(s => s.LastName),
            "lastname_desc" => value.OrderByDescending(s => s.LastName),
            "firstname_asc" => value.OrderBy(s => s.FirstMidName),
            "firstname_desc" => value.OrderByDescending(s => s.FirstMidName),
            "hiredate_asc" => value.OrderBy(s => s.HireDate),
            "hiredate_desc" => value.OrderByDescending(s => s.HireDate),
            _ => value.OrderBy(s => s.LastName),
        };
    }

    public static IQueryable<Course> Sort(this IQueryable<Course> value, string sortString)
    {
        return (sortString?.ToLower() ?? "") switch
        {
            "title_asc" => value.OrderBy(s => s.Title),
            "title_desc" => value.OrderByDescending(s => s.Title),
            "courseid_asc" => value.OrderBy(s => s.CourseID),
            "courseid_desc" => value.OrderByDescending(s => s.CourseID),
            "credits_asc" => value.OrderBy(s => s.Credits),
            "credits_desc" => value.OrderByDescending(s => s.Credits),
            _ => value.OrderBy(s => s.Title),
        };
    }
}
