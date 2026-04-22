using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Courses;

namespace WebUI.IntegrationTests;

public class CoursesControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAll_WithoutCourses_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/courses", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<OverviewDto<CourseOverviewDto>>(TestContext.Current.CancellationToken)).Records.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetAll_WithCourses_ReturnsCourses()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Name 1"
        };

        var course = new Course
        {
            CourseID = 1,
            Title = "Title 1",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            schoolContext.Courses.Add(course);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/courses", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<CourseOverviewDto>>(TestContext.Current.CancellationToken);
        result.Records.Count.ShouldBe(1);
        result.Records[0].CourseId.ShouldBe(course.CourseID);
        result.Records[0].Title.ShouldBe(course.Title);
        result.Records[0].Credits.ShouldBe(course.Credits);
        result.Records[0].DepartmentName.ShouldBe(department.Name);
    }

    [Fact]
    public async Task GetAll_SearchString_ReturnsCoursesWithTitleMatchingSearchString()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Name 1"
        };

        var course1 = new Course
        {
            CourseID = 1,
            Title = "Economics",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course2 = new Course
        {
            CourseID = 2,
            Title = "Math",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            schoolContext.Courses.Add(course1);
            schoolContext.Courses.Add(course2);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var responseLowerCase = await _client.GetAsync("/api/courses?searchString=math", TestContext.Current.CancellationToken);

        responseLowerCase.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await responseLowerCase.Content.ReadAsAsync<OverviewDto<CourseOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(1);
        result.Records[0].CourseId.ShouldBe(course2.CourseID);
        result.Records[0].Title.ShouldBe(course2.Title);
        result.Records[0].Credits.ShouldBe(course2.Credits);
        result.Records[0].DepartmentName.ShouldBe(department.Name);

        var responseUpperCase = await _client.GetAsync("/api/courses?searchString=MATH", TestContext.Current.CancellationToken);
        responseUpperCase.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        var resultUpperCase = await responseUpperCase.Content.ReadAsAsync<OverviewDto<CourseOverviewDto>>(TestContext.Current.CancellationToken);
        resultUpperCase.Records.Count.ShouldBe(1);
    }


    [Fact]
    public async Task GetAll_WithOrder_ReturnsCoursesInCorrectOrder()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Name 1"
        };

        var course1 = new Course
        {
            CourseID = 1,
            Title = "abc",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course2 = new Course
        {
            CourseID = 2,
            Title = "def",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            schoolContext.Courses.Add(course1);
            schoolContext.Courses.Add(course2);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/courses?sortOrder=title_desc", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<CourseOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(2);
        result.Records[0].CourseId.ShouldBe(course2.CourseID);
        result.Records[0].Title.ShouldBe(course2.Title);
        result.Records[0].Credits.ShouldBe(course2.Credits);
        result.Records[0].DepartmentName.ShouldBe(department.Name);
    }

    [Fact]
    public async Task GetAll_WithPageSize_ReturnsOnlyCoursesOnFirstPage()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Name 1"
        };

        var course1 = new Course
        {
            CourseID = 1,
            Title = "abc",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course2 = new Course
        {
            CourseID = 2,
            Title = "def",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course3 = new Course
        {
            CourseID = 3,
            Title = "ghi",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course4 = new Course
        {
            CourseID = 4,
            Title = "jkl",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            schoolContext.Courses.Add(course1);
            schoolContext.Courses.Add(course2);
            schoolContext.Courses.Add(course3);
            schoolContext.Courses.Add(course4);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/courses?pageSize=2", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<CourseOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(2);

        result.Records[0].CourseId.ShouldBe(course1.CourseID);
        result.Records[0].Title.ShouldBe(course1.Title);
        result.Records[0].Credits.ShouldBe(course1.Credits);
        result.Records[0].DepartmentName.ShouldBe(department.Name);

        result.Records[1].CourseId.ShouldBe(course2.CourseID);
        result.Records[1].Title.ShouldBe(course2.Title);
        result.Records[1].Credits.ShouldBe(course2.Credits);
        result.Records[1].DepartmentName.ShouldBe(department.Name);
    }

    [Fact]
    public async Task GetAll_WithPageSizeAndNumber_ReturnsOnlyCoursesOnSecondPage()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Name 1"
        };

        var course1 = new Course
        {
            CourseID = 1,
            Title = "abc",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course2 = new Course
        {
            CourseID = 2,
            Title = "def",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course3 = new Course
        {
            CourseID = 3,
            Title = "ghi",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        var course4 = new Course
        {
            CourseID = 4,
            Title = "jkl",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            schoolContext.Courses.Add(course1);
            schoolContext.Courses.Add(course2);
            schoolContext.Courses.Add(course3);
            schoolContext.Courses.Add(course4);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/courses?pageNumber=1&pageSize=2", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<CourseOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(2);

        result.Records[0].CourseId.ShouldBe(course3.CourseID);
        result.Records[0].Title.ShouldBe(course3.Title);
        result.Records[0].Credits.ShouldBe(course3.Credits);
        result.Records[0].DepartmentName.ShouldBe(department.Name);

        result.Records[1].CourseId.ShouldBe(course4.CourseID);
        result.Records[1].Title.ShouldBe(course4.Title);
        result.Records[1].Credits.ShouldBe(course4.Credits);
        result.Records[1].DepartmentName.ShouldBe(department.Name);
    }

    [Fact]
    public async Task GetSingle_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/courses/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSingle_WithExistingId_ReturnsCourse()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Name 1"
        };

        var course = new Course
        {
            CourseID = 1,
            Title = "Title 1",
            Credits = 2,
            DepartmentID = department.DepartmentID
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            schoolContext.Courses.Add(course);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/courses/1", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<CourseDetailDto>(TestContext.Current.CancellationToken);
        result.CourseId.ShouldBe(course.CourseID);
        result.Title.ShouldBe(course.Title);
        result.Credits.ShouldBe(course.Credits);
        result.DepartmentId.ShouldBe(department.DepartmentID);
    }

    [Fact]
    public async Task Create_CreatesCourse()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Name 1"
        };

        var course = new CreateCourseDto
        {
            CourseId = 1,
            Title = "Title 1",
            Credits = 2,
            DepartmentId = 1
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.PostAsJsonAsync("/api/courses", course, TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Courses.Count().ShouldBe(1);
            schoolContext.Courses.First().CourseID.ShouldBe(course.CourseId);
            schoolContext.Courses.First().Title.ShouldBe(course.Title);
            schoolContext.Courses.First().Credits.ShouldBe(course.Credits);
            schoolContext.Courses.First().DepartmentID.ShouldBe(course.DepartmentId);
        }
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/api/courses/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithExistingId_DeletesCourse()
    {
        using var scope = _appFactory.Services.CreateScope();
        var course = new Course
        {
            CourseID = 1,
            Title = "Test 1",
            Credits = 2,
            DepartmentID = 3
        };

        var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
        schoolContext.Courses.Add(course);
        await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await _client.DeleteAsync("/api/courses/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        schoolContext.Courses.ShouldBeEmpty();
    }


    [Fact]
    public async Task Update_UpdatesCourse()
    {
        using (var scope = _appFactory.Services.CreateScope())
        {
            var course = new Course
            {
                CourseID = 1,
                Title = "Title 1",
                Credits = 2,
                DepartmentID = 3
            };

            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
            schoolContext.Courses.Add(course);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var updateCourseCommand = new UpdateCourseDto
        {
            CourseId = 1,
            Title = "Title 2",
            Credits = 22,
            DepartmentId = 4
        };

        var response = await _client.PutAsJsonAsync("/api/courses", updateCourseCommand);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Courses.Count().ShouldBe(1);
            schoolContext.Courses.First().Title.ShouldBe(updateCourseCommand.Title);
            schoolContext.Courses.First().Credits.ShouldBe(updateCourseCommand.Credits);
            schoolContext.Courses.First().DepartmentID.ShouldBe(updateCourseCommand.DepartmentId);
        }
    }

    [Fact]
    public async Task GetByInstructor_WithoutCourses_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/courses/byinstructor/1", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<CoursesForInstructorOverviewDto>(TestContext.Current.CancellationToken)).Courses.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetByInstructor_WithCourses_ReturnsCoursesForInstructor()
    {
        var instructor = new Instructor
        {
            ID = 1,
            FirstMidName = "Firstname",
            LastName = "Lastname"
        };

        var department = new Department
        {
            DepartmentID = 1,
            Name = "Department name"
        };

        var course = new Course
        {
            CourseID = 1,
            Title = "Title 1",
            DepartmentID = department.DepartmentID
        };

        var courseAssignment = new CourseAssignment
        {
            CourseID = 1,
            InstructorID = 1
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor);
            schoolContext.Departments.Add(department);
            schoolContext.Courses.Add(course);
            schoolContext.CourseAssignments.Add(courseAssignment);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/courses/byinstructor/1", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<CoursesForInstructorOverviewDto>(TestContext.Current.CancellationToken);

        result.Courses.Count.ShouldBe(1);
        result.Courses[0].CourseId.ShouldBe(course.CourseID);
        result.Courses[0].Title.ShouldBe(course.Title);
        result.Courses[0].DepartmentName.ShouldBe(department.Name);
    }
}