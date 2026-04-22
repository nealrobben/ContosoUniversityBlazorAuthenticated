using AutoFixture;
using Bunit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Shouldly;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Students;
using WebUI.Client.Services;
using WebUI.Client.Tests.Extensions;

namespace WebUI.Client.Tests.Pages.Students;

public class StudentTests : BunitTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Students_ClickCreateButton_OpensDialog()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h2").TrimmedText().ShouldBe("Students");
        comp.Find("#CreateButton").ShouldNotBeNull();

        comp.Find("#CreateButton").Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Students_ClickSearch_CallsStudentService()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#SearchButton").ShouldNotBeNull();
        comp.Find("#SearchButton").Click();

        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Students_ClickBackToFullList_CallsStudentService()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#BackToFullListButton").ShouldNotBeNull();
        comp.Find("#BackToFullListButton").Click();

        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Students_ClickDetailsButton_OpensDialog()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(studentsOverviewDto);

        var studentDetailDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailDto.Enrollments = [enrollment];
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);

        Context.Services.AddScoped(x => fakeStudentService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenStudentDetailsButton")[0].ShouldNotBeNull();
        comp.FindAll(".OpenStudentDetailsButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Students_ClickEditButton_OpensDialog()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(studentsOverviewDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenStudentEditButton")[0].ShouldNotBeNull();
        comp.FindAll(".OpenStudentEditButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Students_ClickDeleteButton_ShowsConfirmationDialog()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(studentsOverviewDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".StudentDeleteButton")[0].ShouldNotBeNull();
        comp.FindAll(".StudentDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.Find(".mud-dialog-content").TrimmedText().ShouldBe($"Are you sure you want to delete Student {studentsOverviewDto.Records[0].FullName}?");
    }

    [Fact]
    public void Students_ClickDeleteButtonAndConfirm_StudentServiceShouldBeCalled()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored,
            A<int?>.Ignored, A<CancellationToken>.Ignored)).Returns(studentsOverviewDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = RenderComponent<MudDialogProvider>();

        var comp = RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".StudentDeleteButton")[0].ShouldNotBeNull();
        comp.FindAll(".StudentDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.FindAll("button")[1].ShouldNotBeNull();
        dialog.FindAll("button")[1].Click();

        A.CallTo(() => fakeStudentService.DeleteAsync(A<string>.Ignored)).MustHaveHappened();
    }
}