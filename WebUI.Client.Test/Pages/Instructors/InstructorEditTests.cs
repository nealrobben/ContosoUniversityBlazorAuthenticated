using AngleSharp.Html.Dom;
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.Pages.Instructors;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using Xunit;

namespace WebUI.Client.Test.Pages.Instructors;

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

        var comp = Context.RenderComponent<MudDialogProvider>();
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

        comp.Find("h6").TrimmedText().Should().Be(title);

        ((IHtmlInputElement)comp.FindAll("input")[0]).Value.Should().Be(instructorDetailsDto.LastName);
        ((IHtmlInputElement)comp.FindAll("input")[1]).Value.Should().Be(instructorDetailsDto.FirstName);
        ((IHtmlInputElement)comp.FindAll("input")[2]).Value.Should().Be(instructorDetailsDto.HireDate.ToString("yyyy-MM-dd"));
        ((IHtmlInputElement)comp.FindAll("input")[3]).Value.Should().Be(instructorDetailsDto.OfficeLocation);
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

        var comp = Context.RenderComponent<MudDialogProvider>();
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

        comp.Find("button[type='button']").Click();
        comp.Markup.Trim().Should().BeEmpty();
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

        var comp = Context.RenderComponent<MudDialogProvider>();
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

        comp.Find("#LastName").Change("new lastname");
        comp.Find("#FirstName").Change("new firstname");
        comp.Find("#HireDate").Change("1/3/2021");
        comp.Find("#OfficeLocation").Change("1");

        comp.Find("button[type='submit']").Click();
        comp.Markup.Trim().Should().BeEmpty();
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

        var comp = Context.RenderComponent<MudDialogProvider>();
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

        comp.Find("#LastName").Change("new lastname");
        comp.Find("#FirstName").Change("new firstname");
        comp.Find("#HireDate").Change("1/3/2021");
        comp.Find("#OfficeLocation").Change("1");

        comp.Find("button[type='submit']").Click();
        comp.Markup.Trim().Should().BeEmpty();

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

        var comp = Context.RenderComponent<MudDialogProvider>();
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

        comp.Find("#LastName").Change("new lastname");
        comp.Find("#FirstName").Change("new firstname");
        comp.Find("#HireDate").Change("1/3/2021");
        comp.Find("#OfficeLocation").Change("1");

        comp.Find("button[type='submit']").Click();

        dialog?.ErrorVisible.Should().Be(true);
        comp.Find("div.mud-alert-message").TrimmedText().Should().Be("An error occured during saving");
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

        var comp = Context.RenderComponent<MudDialogProvider>();
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

        comp.Find("#LastName").Change("");
        comp.Find("#FirstName").Change("");
        comp.Find("#HireDate").Change("");

        comp.Find("button[type='submit']").Click();

        comp.FindAll("div.validation-message")[0].TrimmedText().Should().Be("'Last Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().Should().Be("'First Name' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().Should().Be("The HireDate field must be a date.");
    }
}
