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

public class StudentCreateTests : BunitTestBase
{
    [Fact]
    public async Task StudentCreate_DisplayDialogCorrectly()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().ShouldBe(title);

        comp.FindAll("input")[0].Id.ShouldBe("LastName");
        comp.FindAll("input")[1].Id.ShouldBe("FirstName");
        comp.FindAll("input")[2].Id.ShouldBe("EnrollmentDate");
    }

    [Fact]
    public async Task StudentCreate_WhenCancelButtonClicked_PopupCloses()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button[type='button']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task StudentCreate_WhenCreateButtonClicked_PopupCloses()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("lastname");
        await comp.Find("#FirstName").ChangeAsync("Firstname");
        await comp.Find("#EnrollmentDate").ChangeAsync("01/03/2021");

        await comp.Find("button[type='submit']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task StudentCreate_WhenCreateButtonClicked_StudentServiceMustBeCalled()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("lastname");
        await comp.Find("#FirstName").ChangeAsync("Firstname");
        await comp.Find("#EnrollmentDate").ChangeAsync("01/03/2021");

        await comp.Find("button[type='submit']").ClickAsync();

        A.CallTo(() => fakeStudentService.CreateAsync(A<CreateStudentDto>.That.IsInstanceOf(typeof(CreateStudentDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task StudentCreate_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        A.CallTo(() => fakeStudentService.CreateAsync(A<CreateStudentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as StudentCreate;

        await comp.Find("#LastName").ChangeAsync("lastname");
        await comp.Find("#FirstName").ChangeAsync("Firstname");
        await comp.Find("#EnrollmentDate").ChangeAsync("01/03/2021");

        await comp.Find("button[type='submit']").ClickAsync();

        dialog?.ErrorVisible.ShouldBe(true);
        comp.Find("div.mud-alert-message").TrimmedText().ShouldBe("An error occured during saving");
    }

    [Fact]
    public async Task StudentCreate_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var fakeStudentService = A.Fake<IStudentService>();
        Context.Services.AddScoped(x => fakeStudentService);

        var fakeUploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => fakeUploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Student";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<StudentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#EnrollmentDate").ChangeAsync("");

        await comp.Find("button[type='submit']").ClickAsync();

        comp.FindAll("div.validation-message")[0].TrimmedText().ShouldBe("'Last Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().ShouldBe("'First Name' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().ShouldBe("The EnrollmentDate field must be a date.");
    }
}