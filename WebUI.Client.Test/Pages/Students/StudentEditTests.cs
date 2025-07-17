
using AngleSharp.Html.Dom;
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Students;
using WebUI.Client.Pages.Students;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using Xunit;

namespace WebUI.Client.Test.Pages.Students;

public class StudentEditTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public StudentEditTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task StudentDetails_DisplayDetailsCorrectly()
    {
        var studentDetailsDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailsDto.Enrollments = [enrollment];

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailsDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().Should().Be(title);

        ((IHtmlInputElement)comp.FindAll("input")[0]).Value.Should().Be(studentDetailsDto.LastName);
        ((IHtmlInputElement)comp.FindAll("input")[1]).Value.Should().Be(studentDetailsDto.FirstName);
        ((IHtmlInputElement)comp.FindAll("input")[2]).Value.Should().Be(studentDetailsDto.EnrollmentDate.ToString("yyyy-MM-dd"));
    }

    [Fact]
    public async Task StudentDetails_WhenCancelButtonClicked_PopupCloses()
    {
        var studentDetailsDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailsDto.Enrollments = [enrollment];

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailsDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("button[type='button']").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task StudentDetails_WhenEditButtonClicked_PopupCloses()
    {
        var studentDetailsDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailsDto.Enrollments = [enrollment];

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailsDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#LastName").Change("new lastname");
        comp.Find("#FirstName").Change("new firstname");
        comp.Find("#EnrollmentDate").Change("1/3/2021");

        comp.Find("button[type='submit']").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task StudentDetails_WhenEditButtonClicked_StudentServiceMustBeCalled()
    {
        var studentDetailsDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailsDto.Enrollments = [enrollment];

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailsDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#LastName").Change("new lastname");
        comp.Find("#FirstName").Change("new firstname");
        comp.Find("#EnrollmentDate").Change("1/3/2021");

        comp.Find("button[type='submit']").Click();
        comp.Markup.Trim().Should().BeEmpty();

        A.CallTo(() => fakeStudentService.UpdateAsync(A<UpdateStudentDto>.That.IsInstanceOf(typeof(UpdateStudentDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task StudentDetails_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var studentDetailsDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailsDto.Enrollments = [enrollment];

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailsDto);
        A.CallTo(() => fakeStudentService.UpdateAsync(A<UpdateStudentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as StudentEdit;

        comp.Find("#LastName").Change("new lastname");
        comp.Find("#FirstName").Change("new firstname");
        comp.Find("#EnrollmentDate").Change("1/3/2021");

        comp.Find("button[type='submit']").Click();

        dialog?.ErrorVisible.Should().Be(true);
        comp.Find("div.mud-alert-message").TrimmedText().Should().Be("An error occured during saving");
    }

    [Fact]
    public async Task StudentDetails_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var studentDetailsDto = _fixture.Create<StudentDetailDto>();
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        studentDetailsDto.Enrollments = [enrollment];

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailsDto);
        A.CallTo(() => fakeStudentService.UpdateAsync(A<UpdateStudentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#LastName").Change("");
        comp.Find("#FirstName").Change("");
        comp.Find("#EnrollmentDate").Change("");

        comp.Find("button[type='submit']").Click();

        comp.FindAll("div.validation-message")[0].TrimmedText().Should().Be("'Last Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().Should().Be("'First Name' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().Should().Be("The EnrollmentDate field must be a date.");
    }
}
