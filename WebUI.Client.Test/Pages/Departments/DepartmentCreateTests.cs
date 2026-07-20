using Bunit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Shouldly;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.Pages.Departments;
using WebUI.Client.Services;
using WebUI.Client.Tests.Extensions;

namespace WebUI.Client.Tests.Pages.Departments;

public class DepartmentCreateTests : BunitTestBase
{
    [Fact]
    public async Task DepartmentCreate_DisplayDialogCorrectly()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().ShouldBe(title);

        comp.FindAll("input")[0].Id.ShouldBe("Name");
        comp.FindAll("input")[1].Id.ShouldBe("Budget");
        comp.FindAll("input")[2].Id.ShouldBe("StartDate");
        comp.FindAll("input")[3].Id.ShouldBe("InstructorID");
    }

    [Fact]
    public async Task DepartmentCreate_WhenCancelButtonClicked_PopupCloses()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button[type='button']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task DepartmentCreate_WhenCreateButtonClicked_PopupCloses()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#Name").ChangeAsync("My name");
        await comp.Find("#Budget").ChangeAsync("123");
        await comp.Find("#StartDate").ChangeAsync("1/3/2021");
        await comp.Find("#InstructorID").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task DepartmentCreate_WhenCreateButtonClicked_DepartmentServiceMustBeCalled()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#Name").ChangeAsync("My name");
        await comp.Find("#Budget").ChangeAsync("123");
        await comp.Find("#StartDate").ChangeAsync("1/3/2021");
        await comp.Find("#InstructorID").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();

        A.CallTo(() => fakeDepartmentService.CreateAsync(A<CreateDepartmentDto>.That.IsInstanceOf(typeof(CreateDepartmentDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task DepartmentCreate_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.CreateAsync(A<CreateDepartmentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as DepartmentCreate;

        await comp.Find("#Name").ChangeAsync("My name");
        await comp.Find("#Budget").ChangeAsync("123");
        await comp.Find("#StartDate").ChangeAsync("1/3/2021");
        await comp.Find("#InstructorID").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();

        dialog?.ErrorVisible.ShouldBe(true);
        comp.Find("div.mud-alert-message").TrimmedText().ShouldBe("An error occured during saving");
    }

    [Fact]
    public async Task DepartmentCreate_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#StartDate").ChangeAsync("");

        await comp.Find("button[type='submit']").ClickAsync();

        comp.FindAll("div.validation-message")[0].TrimmedText().ShouldBe("'Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().ShouldBe("'Budget' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().ShouldBe("'Budget' must be greater than '0'.");
        comp.FindAll("div.validation-message")[3].TrimmedText().ShouldBe("The StartDate field must be a date.");
    }

    private static InstructorsLookupDto GetInstructorsLookupDtoWithTestData()
    {
        return new InstructorsLookupDto(
        [
            new(1, "Test One"),
            new(2, "Test Two")
        ]);
    }
}