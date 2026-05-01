using AngleSharp.Html.Dom;
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

public class StudentEditTests : BunitTestBase
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

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().ShouldBe(title);

        ((IHtmlInputElement)comp.FindAll("input")[0]).Value.ShouldBe(studentDetailDto.LastName);
        ((IHtmlInputElement)comp.FindAll("input")[1]).Value.ShouldBe(studentDetailDto.FirstName);
        ((IHtmlInputElement)comp.FindAll("input")[2]).Value.ShouldBe(studentDetailDto.EnrollmentDate.ToString("yyyy-MM-dd"));
    }

    [Fact]
    public async Task StudentDetails_WhenCancelButtonClicked_PopupCloses()
    {
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        var studentDetailDto = _fixture
            .Build<StudentDetailDto>()
            .Do(x => x.Enrollments.Add(enrollment))
            .Create();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button[type='button']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task StudentDetails_WhenEditButtonClicked_PopupCloses()
    {
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        var studentDetailDto = _fixture
            .Build<StudentDetailDto>()
            .Do(x => x.Enrollments.Add(enrollment))
            .Create();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("new lastname");
        await comp.Find("#FirstName").ChangeAsync("new firstname");
        await comp.Find("#EnrollmentDate").ChangeAsync("1/3/2021");

        await comp.Find("button[type='submit']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task StudentDetails_WhenEditButtonClicked_StudentServiceMustBeCalled()
    {
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        var studentDetailDto = _fixture
            .Build<StudentDetailDto>()
            .Do(x => x.Enrollments.Add(enrollment))
            .Create();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("new lastname");
        await comp.Find("#FirstName").ChangeAsync("new firstname");
        await comp.Find("#EnrollmentDate").ChangeAsync("1/3/2021");

        await comp.Find("button[type='submit']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();

        A.CallTo(() => fakeStudentService.UpdateAsync(A<UpdateStudentDto>.That.IsInstanceOf(typeof(UpdateStudentDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task StudentDetails_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        var studentDetailDto = _fixture
            .Build<StudentDetailDto>()
            .Do(x => x.Enrollments.Add(enrollment))
            .Create();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);
        A.CallTo(() => fakeStudentService.UpdateAsync(A<UpdateStudentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as StudentEdit;

        await comp.Find("#LastName").ChangeAsync("new lastname");
        await comp.Find("#FirstName").ChangeAsync("new firstname");
        await comp.Find("#EnrollmentDate").ChangeAsync("1/3/2021");

        await comp.Find("button[type='submit']").ClickAsync();

        dialog?.ErrorVisible.ShouldBe(true);
        comp.Find("div.mud-alert-message").TrimmedText().ShouldBe("An error occured during saving");
    }

    [Fact]
    public async Task StudentDetails_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var enrollment = _fixture.Create<StudentDetailEnrollmentDto>();
        var studentDetailDto = _fixture
            .Build<StudentDetailDto>()
            .Do(x => x.Enrollments.Add(enrollment))
            .Create();

        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.GetAsync(A<string>.Ignored)).Returns(studentDetailDto);
        A.CallTo(() => fakeStudentService.UpdateAsync(A<UpdateStudentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeStudentService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "StudentId", 1 }
        };

        const string title = "Edit Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("");
        await comp.Find("#FirstName").ChangeAsync("");
        await comp.Find("#EnrollmentDate").ChangeAsync("");

        await comp.Find("button[type='submit']").ClickAsync();

        comp.FindAll("div.validation-message")[0].TrimmedText().ShouldBe("'Last Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().ShouldBe("'First Name' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().ShouldBe("The EnrollmentDate field must be a date.");
    }
}