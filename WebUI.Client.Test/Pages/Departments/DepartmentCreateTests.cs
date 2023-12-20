
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.Pages.Departments;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using Xunit;

namespace WebUI.Client.Test.Pages.Departments;

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

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().Should().Be(title);

        comp.FindAll("input")[0].Id.Should().Be("Name");
        comp.FindAll("input")[1].Id.Should().Be("Budget");
        comp.FindAll("input")[2].Id.Should().Be("StartDate");
        comp.FindAll("input")[3].Id.Should().Be("InstructorID");
    }

    [Fact]
    public async Task DepartmentCreate_WhenCancelButtonClicked_PopupCloses()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("button[type='button']").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task DepartmentCreate_WhenCreateButtonClicked_PopupCloses()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#Name").Change("My name");
        comp.Find("#Budget").Change("123");
        comp.Find("#StartDate").Change("1/3/2021");
        comp.Find("#InstructorID").Change("1");

        comp.Find("button[type='submit']").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task DepartmentCreate_WhenCreateButtonClicked_DepartmentServiceMustBeCalled()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#Name").Change("My name");
        comp.Find("#Budget").Change("123");
        comp.Find("#StartDate").Change("1/3/2021");
        comp.Find("#InstructorID").Change("1");

        comp.Find("button[type='submit']").Click();

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

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as DepartmentCreate;

        comp.Find("#Name").Change("My name");
        comp.Find("#Budget").Change("123");
        comp.Find("#StartDate").Change("1/3/2021");
        comp.Find("#InstructorID").Change("1");

        comp.Find("button[type='submit']").Click();

        dialog?.ErrorVisible.Should().Be(true);
        comp.Find("div.mud-alert-message").TrimmedText().Should().Be("An error occured during saving");
    }

    [Fact]
    public async Task DepartmentCreate_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#StartDate").Change("");

        comp.Find("button[type='submit']").Click();

        comp.FindAll("div.validation-message")[0].TrimmedText().Should().Be("'Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().Should().Be("'Budget' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().Should().Be("'Budget' must be greater than '0'.");
        comp.FindAll("div.validation-message")[3].TrimmedText().Should().Be("The StartDate field must be a date.");
    }

    private static InstructorsLookupDto GetInstructorsLookupDtoWithTestData()
    {
        return new InstructorsLookupDto(new List<InstructorLookupDto>
        {
            new() {
                ID = 1,
                FullName = "Test One"
            },
            new() {
                ID = 2,
                FullName = "Test Two"
            }
        });
    }
}
