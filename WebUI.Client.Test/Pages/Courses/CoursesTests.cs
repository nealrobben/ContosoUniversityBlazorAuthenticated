using AutoFixture;
using Bunit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Shouldly;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Courses;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Services;
using WebUI.Client.Tests.Extensions;

namespace WebUI.Client.Tests.Pages.Courses;

public class CoursesTests : BunitTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Courses_ClickCreateButton_OpensDialog()
    {
        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fixture = new Fixture();
        var departmentsLookupDto = fixture.Create<DepartmentsLookupDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(departmentsLookupDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h2").TrimmedText().ShouldBe("Courses");
        comp.Find("#CreateButton").ShouldNotBeNull();

        comp.Find("#CreateButton").Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Courses_ClickSearch_CallsCourseService()
    {
        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#SearchButton").ShouldNotBeNull();
        comp.Find("#SearchButton").Click();

        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Courses_ClickBackToFullList_CallsCourseService()
    {
        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#BackToFullListButton").ShouldNotBeNull();
        comp.Find("#BackToFullListButton").Click();

        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Courses_ClickDetailsButton_OpensDialog()
    {
        var coursesOverviewDto = _fixture.Create<OverviewDto<CourseOverviewDto>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(coursesOverviewDto);
        Context.Services.AddScoped(x => fakeCourseService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenCourseDetailsButton")[0].ShouldNotBeNull();
        comp.FindAll(".OpenCourseDetailsButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Courses_ClickEditButton_OpensDialog()
    {
        var coursesOverviewDto = _fixture.Create<OverviewDto<CourseOverviewDto>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(coursesOverviewDto);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenCourseEditButton")[0].ShouldNotBeNull();
        comp.FindAll(".OpenCourseEditButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Courses_ClickDeleteButton_ShowsConfirmationDialog()
    {
        var coursesOverviewDto = _fixture.Create<OverviewDto<CourseOverviewDto>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(coursesOverviewDto);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".CourseDeleteButton")[0].ShouldNotBeNull();
        comp.FindAll(".CourseDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.Find(".mud-dialog-content").TrimmedText().ShouldBe($"Are you sure you want to delete the course {coursesOverviewDto.Records[0].Title}?");
    }

    [Fact]
    public void Courses_ClickDeleteButtonAndConfirm_CourseServiceShouldBeCalled()
    {
        var coursesOverviewDto = _fixture.Create<OverviewDto<CourseOverviewDto>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(coursesOverviewDto);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".CourseDeleteButton")[0].ShouldNotBeNull();
        comp.FindAll(".CourseDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.FindAll("button")[1].ShouldNotBeNull();
        dialog.FindAll("button")[1].Click();

        A.CallTo(() => fakeCourseService.DeleteAsync(A<string>.Ignored)).MustHaveHappened();
    }
}