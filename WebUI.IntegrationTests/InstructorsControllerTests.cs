using Application.Common.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Instructors;

namespace WebUI.IntegrationTests;

public class InstructorsControllerTests : IntegrationTest
{
    [Fact]
    public async Task GetAll_WithoutInstructors_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/instructors");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<OverviewDto<InstructorOverviewDto>>()).Records.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_WithInstructors_ReturnsInstructors()
    {
        var instructor = new Instructor
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name",
            HireDate = DateTime.UtcNow,
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor);
            await schoolContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/instructors");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<InstructorOverviewDto>>());

        result.Records.Should().ContainSingle();

        result.Records[0].InstructorID.Should().Be(instructor.ID);
        result.Records[0].FirstName.Should().Be(instructor.FirstMidName);
        result.Records[0].LastName.Should().Be(instructor.LastName);
        result.Records[0].HireDate.Should().Be(instructor.HireDate);
    }

    [Fact]
    public async Task GetAll_WithSearchString_ReturnsInstructorsWithNameMatchingSearchString()
    {
        var instructor1 = new Instructor
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            HireDate = DateTime.UtcNow,
        };

        var instructor2 = new Instructor
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            HireDate = DateTime.UtcNow,
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor1);
            schoolContext.Instructors.Add(instructor2);
            await schoolContext.SaveChangesAsync();
        }

        var responseLowerCase = await _client.GetAsync("/api/instructors?searchString=de");

        responseLowerCase.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = (await responseLowerCase.Content.ReadAsAsync<OverviewDto<InstructorOverviewDto>>());

        result.Records.Should().ContainSingle();

        result.Records[0].InstructorID.Should().Be(instructor2.ID);
        result.Records[0].FirstName.Should().Be(instructor2.FirstMidName);
        result.Records[0].LastName.Should().Be(instructor2.LastName);
        result.Records[0].HireDate.Should().Be(instructor2.HireDate);

        var responseUpperCase = await _client.GetAsync("/api/instructors?searchString=DE");
        responseUpperCase.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var resultUpperCase = (await responseUpperCase.Content.ReadAsAsync<OverviewDto<InstructorOverviewDto>>());
        resultUpperCase.Records.Should().ContainSingle();
    }

    [Fact]
    public async Task GetAll_WithOrder_ReturnsInstructorsInCorrectOrder()
    {
        var instructor1 = new Instructor
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            HireDate = DateTime.UtcNow,
        };

        var instructor2 = new Instructor
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            HireDate = DateTime.UtcNow,
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor1);
            schoolContext.Instructors.Add(instructor2);
            await schoolContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/instructors?sortOrder=lastname_desc");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<InstructorOverviewDto>>());

        result.Records.Count.Should().Be(2);

        result.Records[0].InstructorID.Should().Be(instructor2.ID);
        result.Records[0].FirstName.Should().Be(instructor2.FirstMidName);
        result.Records[0].LastName.Should().Be(instructor2.LastName);
        result.Records[0].HireDate.Should().Be(instructor2.HireDate);
    }

    [Fact]
    public async Task GetAll_WithPageSize_ReturnsOnlyInstructorsOnFirstPage()
    {
        var instructor1 = new Instructor
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            HireDate = DateTime.UtcNow,
        };

        var instructor2 = new Instructor
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            HireDate = DateTime.UtcNow,
        };

        var instructor3 = new Instructor
        {
            ID = 3,
            FirstMidName = "gh",
            LastName = "i",
            HireDate = DateTime.UtcNow,
        };

        var instructor4 = new Instructor
        {
            ID = 4,
            FirstMidName = "jk",
            LastName = "l",
            HireDate = DateTime.UtcNow,
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor1);
            schoolContext.Instructors.Add(instructor2);
            schoolContext.Instructors.Add(instructor3);
            schoolContext.Instructors.Add(instructor4);
            await schoolContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/instructors?pageSize=2");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<InstructorOverviewDto>>());

        result.Records.Count.Should().Be(2);

        result.Records[0].InstructorID.Should().Be(instructor1.ID);
        result.Records[0].FirstName.Should().Be(instructor1.FirstMidName);
        result.Records[0].LastName.Should().Be(instructor1.LastName);
        result.Records[0].HireDate.Should().Be(instructor1.HireDate);

        result.Records[1].InstructorID.Should().Be(instructor2.ID);
        result.Records[1].FirstName.Should().Be(instructor2.FirstMidName);
        result.Records[1].LastName.Should().Be(instructor2.LastName);
        result.Records[1].HireDate.Should().Be(instructor2.HireDate);
    }

    [Fact]
    public async Task GetAll_WithPageSizeAndPageNumber_ReturnsOnlyInstructorsOnSecondPage()
    {
        var instructor1 = new Instructor
        {
            ID = 1,
            FirstMidName = "ab",
            LastName = "c",
            HireDate = DateTime.UtcNow,
        };

        var instructor2 = new Instructor
        {
            ID = 2,
            FirstMidName = "de",
            LastName = "f",
            HireDate = DateTime.UtcNow,
        };

        var instructor3 = new Instructor
        {
            ID = 3,
            FirstMidName = "gh",
            LastName = "i",
            HireDate = DateTime.UtcNow,
        };

        var instructor4 = new Instructor
        {
            ID = 4,
            FirstMidName = "jk",
            LastName = "l",
            HireDate = DateTime.UtcNow,
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor1);
            schoolContext.Instructors.Add(instructor2);
            schoolContext.Instructors.Add(instructor3);
            schoolContext.Instructors.Add(instructor4);
            await schoolContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/instructors?pageSize=2&pageNumber=1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<OverviewDto<InstructorOverviewDto>>());

        result.Records.Count.Should().Be(2);

        result.Records[0].InstructorID.Should().Be(instructor3.ID);
        result.Records[0].FirstName.Should().Be(instructor3.FirstMidName);
        result.Records[0].LastName.Should().Be(instructor3.LastName);
        result.Records[0].HireDate.Should().Be(instructor3.HireDate);

        result.Records[1].InstructorID.Should().Be(instructor4.ID);
        result.Records[1].FirstName.Should().Be(instructor4.FirstMidName);
        result.Records[1].LastName.Should().Be(instructor4.LastName);
        result.Records[1].HireDate.Should().Be(instructor4.HireDate);
    }

    [Fact]
    public async Task GetSingle_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/courses/1");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSingle_WithExistingId_ReturnsInstructor()
    {
        var instructor = new Instructor
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name",
            HireDate = DateTime.UtcNow
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor);
            await schoolContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/instructors/1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<InstructorDetailDto>());

        result.InstructorID.Should().Be(instructor.ID);
        result.FirstName.Should().Be(instructor.FirstMidName);
        result.LastName.Should().Be(instructor.LastName);
        result.HireDate.Should().Be(instructor.HireDate);
    }

    [Fact]
    public async Task Create_CreatesInstructor()
    {
        var instructor = new CreateInstructorDto
        {
            FirstName = "First name",
            LastName = "Last name",
            HireDate = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/instructors", instructor);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        using var scope = _appFactory.Services.CreateScope();
        var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

        schoolContext.Instructors.Should().ContainSingle();
        schoolContext.Instructors.First().FirstMidName.Should().Be(instructor.FirstName);
        schoolContext.Instructors.First().LastName.Should().Be(instructor.LastName);
        schoolContext.Instructors.First().HireDate.Should().Be(instructor.HireDate);
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/api/instructors/1");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithExistingId_DeletesInstructor()
    {
        using var scope = _appFactory.Services.CreateScope();
        var instructor = new Instructor
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name",
            HireDate = DateTime.UtcNow
        };

        var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
        schoolContext.Instructors.Add(instructor);
        await schoolContext.SaveChangesAsync();

        var response = await _client.DeleteAsync("/api/instructors/1");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        schoolContext.Instructors.Should().BeEmpty();
    }

    [Fact]
    public async Task Update_UpdatesInstructor()
    {
        using (var scope = _appFactory.Services.CreateScope())
        {
            var instructor = new Instructor
            {
                ID = 1,
                FirstMidName = "First name",
                LastName = "Last name",
                HireDate = DateTime.UtcNow
            };

            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();
            schoolContext.Instructors.Add(instructor);
            await schoolContext.SaveChangesAsync();
        }

        var updateInstructorCommand = new UpdateInstructorDto
        {
            InstructorID = 1,
            FirstName = "First name 2",
            LastName = "Last name 2",
            HireDate = DateTime.UtcNow.AddDays(1)
        };

        var response = await _client.PutAsJsonAsync("/api/instructors", updateInstructorCommand);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Should().ContainSingle();
            schoolContext.Instructors.First().ID.Should().Be(updateInstructorCommand.InstructorID);
            schoolContext.Instructors.First().FirstMidName.Should().Be(updateInstructorCommand.FirstName);
            schoolContext.Instructors.First().LastName.Should().Be(updateInstructorCommand.LastName);
            schoolContext.Instructors.First().HireDate.Should().Be(updateInstructorCommand.HireDate);
        }
    }

    [Fact]
    public async Task GetLookup_WithoutInstructors_ReturnsEmptyResponse()
    {
        var response = await _client.GetAsync("/api/instructors/lookup");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<InstructorsLookupDto>()).Instructors.Should().BeEmpty();
    }

    [Fact]
    public async Task GetLookup_WithInstructors_ReturnsInstructors()
    {
        var instructor = new Instructor
        {
            ID = 1,
            FirstMidName = "First name",
            LastName = "Last name"
        };

        using (var scope = _appFactory.Services.CreateScope())
        {
            var schoolContext = scope.ServiceProvider.GetRequiredService<ISchoolContext>();

            schoolContext.Instructors.Add(instructor);
            await schoolContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/instructors/lookup");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = (await response.Content.ReadAsAsync<InstructorsLookupDto>());

        result.Instructors[0].ID.Should().Be(instructor.ID);
        result.Instructors[0].FullName.Should().Be(instructor.FullName);
    }
}
