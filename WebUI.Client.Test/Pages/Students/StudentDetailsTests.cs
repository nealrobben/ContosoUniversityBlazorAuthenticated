using AutoFixture;
using Bunit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Shouldly;
using WebUI.Client.Dtos.Students;
using WebUI.Client.Pages.Students;
using WebUI.Client.Services;
using WebUI.Client.Tests.Extensions;

namespace WebUI.Client.Tests.Pages.Students;

public class StudentDetailsTests : BunitTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task StudentDetails_DisplayDetailsCorrectly()
    {
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        var studentDetailDto = _fixture
            .Build<StudentDetailDto>()
            .Do(x => x.Enrollments.Add(enrollment))
            .Create();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Student Details";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().ShouldBe(title);

        comp.FindAll("dt")[0].TrimmedText().ShouldBe("Last name");
        comp.FindAll("dt")[1].TrimmedText().ShouldBe("First name");
        comp.FindAll("dt")[2].TrimmedText().ShouldBe("Enrollment date");

        comp.FindAll("dd")[0].TrimmedText().ShouldBe(studentDetailDto.LastName);
        comp.FindAll("dd")[1].TrimmedText().ShouldBe(studentDetailDto.FirstName);
        comp.FindAll("dd")[2].TrimmedText().ShouldBe(studentDetailDto.EnrollmentDate.ToShortDateString());

        var enrollmentsTable = comp.Find("table");
        var row = ((AngleSharp.Html.Dom.IHtmlTableSectionElement)enrollmentsTable.Children[0]).Rows[1];
        row.Cells[0].TrimmedText().ShouldBe(enrollment.CourseTitle);
        row.Cells[1].TrimmedText().ShouldBe(enrollment.Grade.ToString());
    }

    [Fact]
    public async Task StudentDetails_WhenOkButtonClicked_PopupCloses()
    {
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        var studentDetailDto = _fixture
            .Build<StudentDetailDto>()
            .Do(x => x.Enrollments.Add(enrollment))
            .Create();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Student Details";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }
}