﻿
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using WebUI.Shared.Common;
using WebUI.Shared.Courses.Queries.GetCoursesOverview;
using WebUI.Shared.Departments.Queries.GetDepartmentsLookup;
using Xunit;

namespace WebUI.Client.Test.Pages.Courses;

public class CoursesTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public CoursesTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Courses_ClickCreateButton_OpensDialog()
    {
        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var fixture = new Fixture();
        var departmentsLookupVM = fixture.Create<DepartmentsLookupVM>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(departmentsLookupVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h2").TrimmedText().Should().Be("Courses");
        comp.Find("#CreateButton").Should().NotBeNull();

        comp.Find("#CreateButton").Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Courses_ClickSearch_CallsCourseService()
    {
        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#SearchButton").Should().NotBeNull();
        comp.Find("#SearchButton").Click();

        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Courses_ClickBackToFullList_CallsCourseService()
    {
        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#BackToFullListButton").Should().NotBeNull();
        comp.Find("#BackToFullListButton").Click();

        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Courses_ClickDetailsButton_OpensDialog()
    {
        var coursesOverviewVM = _fixture.Create<OverviewVM<CourseVM>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(coursesOverviewVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenCourseDetailsButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenCourseDetailsButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Courses_ClickEditButton_OpensDialog()
    {
        var coursesOverviewVM = _fixture.Create<OverviewVM<CourseVM>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(coursesOverviewVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenCourseEditButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenCourseEditButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Courses_ClickDeleteButton_ShowsConfirmationDialog()
    {
        var coursesOverviewVM = _fixture.Create<OverviewVM<CourseVM>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(coursesOverviewVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".CourseDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".CourseDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.Find(".mud-dialog-content").TrimmedText().Should().Be($"Are you sure you want to delete the course {coursesOverviewVM.Records[0].Title}?");
    }

    [Fact]
    public void Courses_ClickDeleteButtonAndConfirm_CourseServiceShouldBeCalled()
    {
        var coursesOverviewVM = _fixture.Create<OverviewVM<CourseVM>>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(coursesOverviewVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Courses.Courses>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".CourseDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".CourseDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.FindAll("button")[1].Should().NotBeNull();
        dialog.FindAll("button")[1].Click();

        A.CallTo(() => fakeCourseService.DeleteAsync(A<string>.Ignored)).MustHaveHappened();
    }
}
