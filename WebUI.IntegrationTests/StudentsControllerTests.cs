using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Students;

namespace WebUI.IntegrationTests;

public class StudentsControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAll_WithoutStudents_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/students", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<OverviewDto<StudentOverviewDto>>(TestContext.Current.CancellationToken)).Records.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetAll_WitStudents_ReturnsStudents()
    {
        var student = new Student
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name",
            EnrollmentDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Add(student);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/students", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<StudentOverviewDto>>(TestContext.Current.CancellationToken));

        result.Records.Count.ShouldBe(1);
        result.Records[0].StudentId.ShouldBe(student.ID);
        result.Records[0].FirstName.ShouldBe(student.FirstMidName);
        result.Records[0].LastName.ShouldBe(student.LastName);
        result.Records[0].EnrollmentDate.ShouldBe(student.EnrollmentDate);
    }

    [Fact]
    public async Task GetAll_WithSearchString_ReturnsStudentsWithNameMatchingSearchString()
    {
        var student1 = new Student
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            EnrollmentDate = DateTime.UtcNow
        };

        var student2 = new Student
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            EnrollmentDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Add(student1);
            schoolContext.Students.Add(student2);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var responseLowerCase = await _client.GetAsync("/api/students?searchString=de", TestContext.Current.CancellationToken);

        responseLowerCase.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = (await responseLowerCase.Content.ReadAsAsync<OverviewDto<StudentOverviewDto>>(TestContext.Current.CancellationToken));

        result.Records.Count.ShouldBe(1);

        result.Records[0].StudentId.ShouldBe(student2.ID);
        result.Records[0].FirstName.ShouldBe(student2.FirstMidName);
        result.Records[0].LastName.ShouldBe(student2.LastName);
        result.Records[0].EnrollmentDate.ShouldBe(student2.EnrollmentDate);

        var responseUpperCase = await _client.GetAsync("/api/students?searchString=DE", TestContext.Current.CancellationToken);
        responseUpperCase.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        var resultUpperCase = (await responseUpperCase.Content.ReadAsAsync<OverviewDto<StudentOverviewDto>>(TestContext.Current.CancellationToken));
        resultUpperCase.Records.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetAll_WithOrder_ReturnsStudentsInCorrectOrder()
    {
        var student1 = new Student
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            EnrollmentDate = DateTime.UtcNow
        };

        var student2 = new Student
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            EnrollmentDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Add(student1);
            schoolContext.Students.Add(student2);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/students?sortOrder=lastname_desc", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<StudentOverviewDto>>(TestContext.Current.CancellationToken));

        result.Records.Count.ShouldBe(2);

        result.Records[0].StudentId.ShouldBe(student2.ID);
        result.Records[0].FirstName.ShouldBe(student2.FirstMidName);
        result.Records[0].LastName.ShouldBe(student2.LastName);
        result.Records[0].EnrollmentDate.ShouldBe(student2.EnrollmentDate);

        result.Records[1].StudentId.ShouldBe(student1.ID);
        result.Records[1].FirstName.ShouldBe(student1.FirstMidName);
        result.Records[1].LastName.ShouldBe(student1.LastName);
        result.Records[1].EnrollmentDate.ShouldBe(student1.EnrollmentDate);
    }

    [Fact]
    public async Task GetAll_WithPageSize_ReturnsOnlyStudentsOnFirstPage()
    {
        var student1 = new Student
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            EnrollmentDate = DateTime.UtcNow
        };

        var student2 = new Student
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            EnrollmentDate = DateTime.UtcNow
        };

        var student3 = new Student
        {
            ID = 3,
            FirstMidName = "gh",
            LastName = "i",
            EnrollmentDate = DateTime.UtcNow
        };

        var student4 = new Student
        {
            ID = 4,
            FirstMidName = "jk",
            LastName = "l",
            EnrollmentDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Add(student1);
            schoolContext.Students.Add(student2);
            schoolContext.Students.Add(student3);
            schoolContext.Students.Add(student4);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/students?pageSize=2", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<StudentOverviewDto>>(TestContext.Current.CancellationToken));

        result.Records.Count.ShouldBe(2);

        result.Records[0].StudentId.ShouldBe(student1.ID);
        result.Records[0].FirstName.ShouldBe(student1.FirstMidName);
        result.Records[0].LastName.ShouldBe(student1.LastName);
        result.Records[0].EnrollmentDate.ShouldBe(student1.EnrollmentDate);

        result.Records[1].StudentId.ShouldBe(student2.ID);
        result.Records[1].FirstName.ShouldBe(student2.FirstMidName);
        result.Records[1].LastName.ShouldBe(student2.LastName);
        result.Records[1].EnrollmentDate.ShouldBe(student2.EnrollmentDate);
    }

    [Fact]
    public async Task GetAll_WithPageSizeAndPageNumber_ReturnsOnlyStudentsOnSecondPage()
    {
        var student1 = new Student
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            EnrollmentDate = DateTime.UtcNow
        };

        var student2 = new Student
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            EnrollmentDate = DateTime.UtcNow
        };

        var student3 = new Student
        {
            ID = 3,
            FirstMidName = "gh",
            LastName = "i",
            EnrollmentDate = DateTime.UtcNow
        };

        var student4 = new Student
        {
            ID = 4,
            FirstMidName = "jk",
            LastName = "l",
            EnrollmentDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Add(student1);
            schoolContext.Students.Add(student2);
            schoolContext.Students.Add(student3);
            schoolContext.Students.Add(student4);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/students?pageSize=2&pageNumber=1", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<StudentOverviewDto>>(TestContext.Current.CancellationToken));

        result.Records.Count.ShouldBe(2);

        result.Records[0].StudentId.ShouldBe(student3.ID);
        result.Records[0].FirstName.ShouldBe(student3.FirstMidName);
        result.Records[0].LastName.ShouldBe(student3.LastName);
        result.Records[0].EnrollmentDate.ShouldBe(student3.EnrollmentDate);

        result.Records[1].StudentId.ShouldBe(student4.ID);
        result.Records[1].FirstName.ShouldBe(student4.FirstMidName);
        result.Records[1].LastName.ShouldBe(student4.LastName);
        result.Records[1].EnrollmentDate.ShouldBe(student4.EnrollmentDate);
    }

    [Fact]
    public async Task GetSingle_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/students/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSingle_WithExistingId_ReturnsStudent()
    {
        var student = new Student
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name",
            EnrollmentDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Add(student);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/students/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<StudentDetailDto>(TestContext.Current.CancellationToken);

        result.StudentID.ShouldBe(student.ID);
        result.FirstName.ShouldBe(student.FirstMidName);
        result.LastName.ShouldBe(student.LastName);
        result.EnrollmentDate.ShouldBe(student.EnrollmentDate);
    }

    [Fact]
    public async Task Create_CreatesStudent()
    {
        var student = new CreateStudentDto("First name", "Last name", DateTime.UtcNow, null);

        var response = await _client.PostAsJsonAsync("/api/students", student);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        using var scope = _appFactory.Services.CreateScope();
        var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

        schoolContext.Students.Count().ShouldBe(1);
        schoolContext.Students.First().FirstMidName.ShouldBe(student.FirstName);
        schoolContext.Students.First().LastName.ShouldBe(student.LastName);
        schoolContext.Students.First().EnrollmentDate.ShouldBe(student.EnrollmentDate);
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/api/students/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithExistingId_DeletesStudent()
    {
        using var scope = _appFactory.Services.CreateScope();
        var student = new Student
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name",
            EnrollmentDate = DateTime.UtcNow
        };

        var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
        schoolContext.Students.Add(student);
        await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await _client.DeleteAsync("/api/students/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        schoolContext.Students.ShouldBeEmpty();
    }

    [Fact]
    public async Task Update_UpdatesStudent()
    {
        using (var scope = _appFactory.Services.CreateScope())
        {
            var student = new Student
            {
                ID = 1,
                FirstMidName = "First name",
                LastName = "Last name",
                EnrollmentDate = DateTime.UtcNow
            };

            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
            schoolContext.Students.Add(student);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var updateStudentCommand = new UpdateStudentDto(1, "First name 2", "Last name 2", DateTime.UtcNow.AddDays(1), null);

        var response = await _client.PutAsJsonAsync("/api/students", updateStudentCommand);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Count().ShouldBe(1);
            schoolContext.Students.First().ID.ShouldBe(updateStudentCommand.StudentId!.Value);
            schoolContext.Students.First().FirstMidName.ShouldBe(updateStudentCommand.FirstName);
            schoolContext.Students.First().LastName.ShouldBe(updateStudentCommand.LastName);
            schoolContext.Students.First().EnrollmentDate.ShouldBe(updateStudentCommand.EnrollmentDate);
        }
    }

    [Fact]
    public async Task ByCourse_WithoutStudents_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/students/bycourse/1", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<StudentsForCourseDto>(TestContext.Current.CancellationToken)).Students.ShouldBeEmpty();
    }

    [Fact]
    public async Task ByCourse_WithStudents_ReturnsStudentsForCourse()
    {
        var student = new Student
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name",
            EnrollmentDate = DateTime.UtcNow
        };

        var course = new Course
        {
            CourseID = 1,
            Title = "Title",
            Credits = 2
        };

        var enrollment = new Enrollment
        {
            CourseID = course.CourseID,
            StudentID = student.ID,
            Grade = Grade.B
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Students.Add(student);
            schoolContext.Courses.Add(course);
            schoolContext.Enrollments.Add(enrollment);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/students/bycourse/1", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<StudentsForCourseDto>(TestContext.Current.CancellationToken);
        result.Students.Count.ShouldBe(1);
        result.Students[0].StudentName.ShouldBe(student.FullName);
        result.Students[0].StudentGrade.ShouldBe((int?)enrollment.Grade);
    }
}