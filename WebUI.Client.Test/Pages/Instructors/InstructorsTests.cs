
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Courses;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.Dtos.Students;
using WebUI.Client.Enums;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using Xunit;

namespace WebUI.Client.Test.Pages.Instructors;

public class InstructorsTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public InstructorsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Instructors_ClickCreateButton_OpensDialog()
    {
        var fakeInstructorService = A.Fake<IInstructorService>();
        Context.Services.AddScoped(x => fakeInstructorService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h2").TrimmedText().Should().Be("Instructors");
        comp.Find("#CreateButton").Should().NotBeNull();

        comp.Find("#CreateButton").Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Instructors_ClickSearch_CallsInstructorService()
    {
        var fakeInstructorService = A.Fake<IInstructorService>();
        Context.Services.AddScoped(x => fakeInstructorService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#SearchButton").Should().NotBeNull();
        comp.Find("#SearchButton").Click();

        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Instructors_ClickBackToFullList_CallsInstructorService()
    {
        var fakeInstructorService = A.Fake<IInstructorService>();
        Context.Services.AddScoped(x => fakeInstructorService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#BackToFullListButton").Should().NotBeNull();
        comp.Find("#BackToFullListButton").Click();

        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Instructor_ClickDetailsButton_OpensDialog()
    {
        var instructorsOverviewDto = _fixture.Create<OverviewDto<InstructorOverviewDto>>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(instructorsOverviewDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenInstructorDetailsButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenInstructorDetailsButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Instructor_ClickEditButton_OpensDialog()
    {
        var instructorsOverviewDto = _fixture.Create<OverviewDto<InstructorOverviewDto>>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(instructorsOverviewDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenInstructorEditButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenInstructorEditButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Instructor_ClickDeleteButton_ShowsConfirmationDialog()
    {
        var instructorsOverviewDto = _fixture.Create<OverviewDto<InstructorOverviewDto>>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(instructorsOverviewDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".InstructorDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".InstructorDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.Find(".mud-dialog-content").TrimmedText().Should().Be($"Are you sure you want to delete Instructor {instructorsOverviewDto.Records[0].FullName}?");
    }

    [Fact]
    public void Instructor_ClickDeleteButtonAndConfirm_InstructorServiceShouldBeCalled()
    {
        var instructorsOverviewDto = _fixture.Create<OverviewDto<InstructorOverviewDto>>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(instructorsOverviewDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".InstructorDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".InstructorDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.FindAll("button")[1].Should().NotBeNull();
        dialog.FindAll("button")[1].Click();

        A.CallTo(() => fakeInstructorService.DeleteAsync(A<string>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Instructor_ClickSelectButton_ShowsCoursesForInstructor()
    {
        var instructorsOverviewDto = _fixture.Create<OverviewDto<InstructorOverviewDto>>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(instructorsOverviewDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var coursesForInstructorOverview = _fixture.Create<CoursesForInstructorOverviewDto>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetCoursesForInstructor(A<string>.Ignored)).Returns(coursesForInstructorOverview);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll("#CoursesForInstructorTable").Should().BeEmpty();

        comp.FindAll(".InstructorSelectButton")[0].Should().NotBeNull();
        comp.FindAll(".InstructorSelectButton")[0].Click();

        comp.FindAll("#CoursesForInstructorTable").Should().NotBeEmpty();
    }

    [Fact]
    public void Instructor_ClickSelectButtonOnCoursesForInstructor_ShowsStudentsForCourse()
    {
        var instructorsOverviewDto = _fixture.Create<OverviewDto<InstructorOverviewDto>>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(instructorsOverviewDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var coursesForInstructorOverview = _fixture.Create<CoursesForInstructorOverviewDto>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetCoursesForInstructor(A<string>.Ignored)).Returns(coursesForInstructorOverview);
        Context.Services.AddScoped(x => fakeCourseService);

        var studentsForCourseDto = new StudentsForCourseDto
        {
            Students =
            {
                new StudentForCourseDto
                {
                    StudentName = "Student X",
                    StudentGrade = (int ?) Grade.A
                }
            }
        };

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetStudentsForCourse("2")).Returns(studentsForCourseDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.RenderComponent<Client.Pages.Instructors.Instructors>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".InstructorSelectButton")[0].Should().NotBeNull();
        comp.FindAll(".InstructorSelectButton")[0].Click();

        comp.FindAll("#StudentsForCourse").Should().BeEmpty();

        comp.FindAll(".CourseSelectButton")[0].Should().NotBeNull();
        comp.FindAll(".CourseSelectButton")[0].Click();

        comp.FindAll("#StudentsForCourse").Should().NotBeEmpty();
    }
}
