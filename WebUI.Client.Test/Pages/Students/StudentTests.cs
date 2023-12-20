
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Common;
using WebUI.Client.Dtos.Students;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using Xunit;

namespace WebUI.Client.Test.Pages.Students;

public class StudentTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public StudentTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Students_ClickCreateButton_OpensDialog()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileuploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h2").TrimmedText().Should().Be("Students");
        comp.Find("#CreateButton").Should().NotBeNull();

        comp.Find("#CreateButton").Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Students_ClickSearch_CallsStudentService()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#SearchButton").Should().NotBeNull();
        comp.Find("#SearchButton").Click();

        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Students_ClickBackToFullList_CallsStudentService()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var comp = Context.RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#BackToFullListButton").Should().NotBeNull();
        comp.Find("#BackToFullListButton").Click();

        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Students_ClickDetailsButton_OpensDialog()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(studentsOverviewDto);

        var studentDetailDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailDto.Enrollments = [enrollment];
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);

        Context.Services.AddScoped(x => fakeStudentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenStudentDetailsButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenStudentDetailsButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Students_ClickEditButton_OpensDialog()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(studentsOverviewDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileuploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenStudentEditButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenStudentEditButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Students_ClickDeleteButton_ShowsConfirmationDialog()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(studentsOverviewDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileuploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".StudentDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".StudentDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.Find(".mud-dialog-content").TrimmedText().Should().Be($"Are you sure you want to delete Student {studentsOverviewDto.Records[0].FullName}?");
    }

    [Fact]
    public void Students_ClickDeleteButtonAndConfirm_StudentServiceShouldBeCalled()
    {
        var studentsOverviewDto = _fixture.Create<OverviewDto<StudentOverviewDto>>();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(studentsOverviewDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileuploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Students.Students>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".StudentDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".StudentDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.FindAll("button")[1].Should().NotBeNull();
        dialog.FindAll("button")[1].Click();

        A.CallTo(() => fakeStudentService.DeleteAsync(A<string>.Ignored)).MustHaveHappened();
    }
}
