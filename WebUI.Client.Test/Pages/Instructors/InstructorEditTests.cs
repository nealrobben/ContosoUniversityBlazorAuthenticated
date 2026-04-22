using AngleSharp.Html.Dom;
using AutoFixture;
using Bunit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Shouldly;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.Pages.Instructors;
using WebUI.Client.Services;
using WebUI.Client.Tests.Extensions;

namespace WebUI.Client.Tests.Pages.Instructors;

public class InstructorEditTests : BunitTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task InstructorEdit_DisplayDialogCorrectly()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Edit Instructor";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().ShouldBe(title);

        ((IHtmlInputElement)comp.FindAll("input")[0]).Value.ShouldBe(instructorDetailsDto.LastName);
        ((IHtmlInputElement)comp.FindAll("input")[1]).Value.ShouldBe(instructorDetailsDto.FirstName);
        ((IHtmlInputElement)comp.FindAll("input")[2]).Value.ShouldBe(instructorDetailsDto.HireDate.ToString("yyyy-MM-dd"));
        ((IHtmlInputElement)comp.FindAll("input")[3]).Value.ShouldBe(instructorDetailsDto.OfficeLocation);
    }

    [Fact]
    public async Task InstructorEdit_WhenCancelButtonClicked_PopupCloses()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Edit Instructor";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button[type='button']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task InstructorEdit_WhenEditButtonClicked_PopupCloses()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Edit Instructor";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("new lastname");
        await comp.Find("#FirstName").ChangeAsync("new firstname");
        await comp.Find("#HireDate").ChangeAsync("1/3/2021");
        await comp.Find("#OfficeLocation").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task InstructorEdit_WhenEditButtonClicked_InstructorServiceMustBeCalled()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Edit Instructor";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("new lastname");
        await comp.Find("#FirstName").ChangeAsync("new firstname");
        await comp.Find("#HireDate").ChangeAsync("1/3/2021");
        await comp.Find("#OfficeLocation").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();

        A.CallTo(() => fakeInstructorService.UpdateAsync(A<UpdateInstructorDto>.That.IsInstanceOf(typeof(UpdateInstructorDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task InstructorEdit_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        A.CallTo(() => fakeInstructorService.UpdateAsync(A<UpdateInstructorDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeInstructorService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Edit Instructor";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as InstructorEdit;

        await comp.Find("#LastName").ChangeAsync("new lastname");
        await comp.Find("#FirstName").ChangeAsync("new firstname");
        await comp.Find("#HireDate").ChangeAsync("1/3/2021");
        await comp.Find("#OfficeLocation").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();

        dialog?.ErrorVisible.ShouldBe(true);
        comp.Find("div.mud-alert-message").TrimmedText().ShouldBe("An error occured during saving");
    }

    [Fact]
    public async Task InstructorEdit_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        A.CallTo(() => fakeInstructorService.UpdateAsync(A<UpdateInstructorDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeInstructorService);

        var uploadService = A.Fake<IFileUploadService>();
        Context.Services.AddScoped(x => uploadService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Edit Instructor";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#LastName").ChangeAsync("");
        await comp.Find("#FirstName").ChangeAsync("");
        await comp.Find("#HireDate").ChangeAsync("");

        await comp.Find("button[type='submit']").ClickAsync();

        comp.FindAll("div.validation-message")[0].TrimmedText().ShouldBe("'Last Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().ShouldBe("'First Name' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().ShouldBe("The HireDate field must be a date.");
    }
}