using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Departments;

namespace WebUI.IntegrationTests;

public class DepartmentsControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAll_WithoutDepartments_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/departments", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<OverviewDto<DepartmentOverviewDto>>(TestContext.Current.CancellationToken)).Records.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetAll_WithDepartments_ReturnsDepartments()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Test 1",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/departments", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<DepartmentOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(1);
        result.Records[0].DepartmentID.ShouldBe(department.DepartmentID);
        result.Records[0].Name.ShouldBe(department.Name);
        result.Records[0].Budget.ShouldBe(department.Budget);
        result.Records[0].StartDate.ShouldBe(department.StartDate);
    }

    [Fact]
    public async Task GetAll_WithSearchString_ReturnsDepartmentsWithNameMatchingSearchString()
    {
        var department1 = new Department
        {
            DepartmentID = 1,
            Name = "abc",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department2 = new Department
        {
            DepartmentID = 2,
            Name = "def",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department1);
            schoolContext.Departments.Add(department2);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var responseLowerCase = await _client.GetAsync("/api/departments?searchString=ef", TestContext.Current.CancellationToken);

        responseLowerCase.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await responseLowerCase.Content.ReadAsAsync<OverviewDto<DepartmentOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(1);
        result.Records[0].DepartmentID.ShouldBe(department2.DepartmentID);
        result.Records[0].Name.ShouldBe(department2.Name);
        result.Records[0].Budget.ShouldBe(department2.Budget);
        result.Records[0].StartDate.ShouldBe(department2.StartDate);

        var responseUpperCase = await _client.GetAsync("/api/departments?searchString=EF", TestContext.Current.CancellationToken);
        responseUpperCase.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        var resultUpperCase = await responseUpperCase.Content.ReadAsAsync<OverviewDto<DepartmentOverviewDto>>(TestContext.Current.CancellationToken);
        resultUpperCase.Records.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetAll_WithOrder_ReturnsDepartmentsInCorrectOrder()
    {
        var department1 = new Department
        {
            DepartmentID = 1,
            Name = "abc",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department2 = new Department
        {
            DepartmentID = 2,
            Name = "def",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department1);
            schoolContext.Departments.Add(department2);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/departments?sortOrder=name_desc", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<DepartmentOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(2);
        result.Records[0].DepartmentID.ShouldBe(department2.DepartmentID);
        result.Records[0].Name.ShouldBe(department2.Name);
        result.Records[0].Budget.ShouldBe(department2.Budget);
        result.Records[0].StartDate.ShouldBe(department2.StartDate);
    }

    [Fact]
    public async Task GetAll_WithPageSize_ReturnsOnlyDepartmentsOnFirstPage()
    {
        var department1 = new Department
        {
            DepartmentID = 1,
            Name = "abc",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department2 = new Department
        {
            DepartmentID = 2,
            Name = "def",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department3 = new Department
        {
            DepartmentID = 3,
            Name = "ghi",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department4 = new Department
        {
            DepartmentID = 4,
            Name = "jkl",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department1);
            schoolContext.Departments.Add(department2);
            schoolContext.Departments.Add(department3);
            schoolContext.Departments.Add(department4);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/departments?pageSize=2", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<DepartmentOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(2);

        result.Records[0].DepartmentID.ShouldBe(department1.DepartmentID);
        result.Records[0].Name.ShouldBe(department1.Name);
        result.Records[0].Budget.ShouldBe(department1.Budget);
        result.Records[0].StartDate.ShouldBe(department1.StartDate);

        result.Records[1].DepartmentID.ShouldBe(department2.DepartmentID);
        result.Records[1].Name.ShouldBe(department2.Name);
        result.Records[1].Budget.ShouldBe(department2.Budget);
        result.Records[1].StartDate.ShouldBe(department2.StartDate);
    }

    [Fact]
    public async Task GetAll_WithPageSizeAndPageNumber_ReturnsOnlyDepartmentsOnSecondPage()
    {
        var department1 = new Department
        {
            DepartmentID = 1,
            Name = "abc",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department2 = new Department
        {
            DepartmentID = 2,
            Name = "def",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department3 = new Department
        {
            DepartmentID = 3,
            Name = "ghi",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var department4 = new Department
        {
            DepartmentID = 4,
            Name = "jkl",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department1);
            schoolContext.Departments.Add(department2);
            schoolContext.Departments.Add(department3);
            schoolContext.Departments.Add(department4);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/departments?pageNumber=1&pageSize=2", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<OverviewDto<DepartmentOverviewDto>>(TestContext.Current.CancellationToken);

        result.Records.Count.ShouldBe(2);

        result.Records[0].DepartmentID.ShouldBe(department3.DepartmentID);
        result.Records[0].Name.ShouldBe(department3.Name);
        result.Records[0].Budget.ShouldBe(department3.Budget);
        result.Records[0].StartDate.ShouldBe(department3.StartDate);

        result.Records[1].DepartmentID.ShouldBe(department4.DepartmentID);
        result.Records[1].Name.ShouldBe(department4.Name);
        result.Records[1].Budget.ShouldBe(department4.Budget);
        result.Records[1].StartDate.ShouldBe(department4.StartDate);
    }

    [Fact]
    public async Task GetSingle_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/departments/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSingle_WithExistingId_ReturnsDepartment()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Test 1",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/departments/1", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<DepartmentDetailDto>(TestContext.Current.CancellationToken);
        result.DepartmentId.ShouldBe(department.DepartmentID);
        result.Name.ShouldBe(department.Name);
        result.Budget.ShouldBe(department.Budget);
        result.StartDate.ShouldBe(department.StartDate);
    }

    [Fact]
    public async Task Create_CreatesDepartment()
    {
        var department = new CreateDepartmentDto
        {
            Name = "Test 1",
            Budget = 123,
            StartDate = DateTime.UtcNow,
            InstructorId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/departments", department);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        using var scope = _appFactory.Services.CreateScope();
        var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

        schoolContext.Departments.Count().ShouldBe(1);
        schoolContext.Departments.First().Name.ShouldBe(department.Name);
        schoolContext.Departments.First().Budget.ShouldBe(department.Budget);
        schoolContext.Departments.First().StartDate.ShouldBe(department.StartDate);
        schoolContext.Departments.First().InstructorID.ShouldBe(department.InstructorId);
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/api/departments/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithExistingId_DeletesDepartment()
    {
        using var scope = _appFactory.Services.CreateScope();
        var department = new Department
        {
            Name = "Test 1",
            Budget = 123,
            StartDate = DateTime.UtcNow
        };

        var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
        schoolContext.Departments.Add(department);
        await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await _client.DeleteAsync("/api/departments/1", TestContext.Current.CancellationToken);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        schoolContext.Departments.ShouldBeEmpty();
    }

    [Fact]
    public async Task Update_UpdatesDepartment()
    {
        using (var scope = _appFactory.Services.CreateScope())
        {
            var department = new Department
            {
                DepartmentID = 1,
                Name = "Test 1",
                Budget = 123,
                StartDate = DateTime.UtcNow,
                InstructorID = 1
            };

            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
            schoolContext.Departments.Add(department);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var updateDepartmentCommand = new UpdateDepartmentDto
        {
            DepartmentId = 1,
            Name = "Test 2",
            Budget = 456,
            StartDate = DateTime.UtcNow.AddDays(1),
            InstructorId = 2
        };

        var response = await _client.PutAsJsonAsync("/api/departments", updateDepartmentCommand);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Count().ShouldBe(1);
            schoolContext.Departments.First().Name.ShouldBe(updateDepartmentCommand.Name);
            schoolContext.Departments.First().Budget.ShouldBe(updateDepartmentCommand.Budget);
            schoolContext.Departments.First().StartDate.ShouldBe(updateDepartmentCommand.StartDate);
            schoolContext.Departments.First().InstructorID.ShouldBe(updateDepartmentCommand.InstructorId);
        }
    }

    [Fact]
    public async Task GetLookup_WithoutDepartments_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/departments/lookup", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<DepartmentsLookupDto>(TestContext.Current.CancellationToken)).Departments.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetLookup_WithDepartments_ReturnsDepartments()
    {
        var department = new Department
        {
            DepartmentID = 1,
            Name = "Test 1"
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Departments.Add(department);
            await schoolContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        var response = await _client.GetAsync("/api/departments/lookup", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadAsAsync<DepartmentsLookupDto>(TestContext.Current.CancellationToken);
        result.Departments.Count.ShouldBe(1);
        result.Departments[0].DepartmentID.ShouldBe(department.DepartmentID);
        result.Departments[0].Name.ShouldBe(department.Name);
    }
}