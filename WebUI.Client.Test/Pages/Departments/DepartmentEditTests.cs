
using AngleSharp.Html.Dom;
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Pages.Departments;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using WebUI.Shared.Departments.Queries.GetDepartmentDetails;
using WebUI.Shared.Instructors.Queries.GetInstructorsLookup;
using Xunit;

namespace WebUI.Client.Test.Pages.Departments;

public class DepartmentEditTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public DepartmentEditTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task DepartmentEdit_DisplayDialogCorrectly()
    {
        var departmentDetailVM = _fixture.Create<DepartmentDetailVM>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().Should().Be(title);

        ((IHtmlInputElement)comp.FindAll("input")[0]).Value.Should().Be(departmentDetailVM.Name);
        ((IHtmlInputElement)comp.FindAll("input")[1]).Value.Should().Be(departmentDetailVM.Budget.ToString());
        ((IHtmlInputElement)comp.FindAll("input")[2]).Value.Should().Be(departmentDetailVM.StartDate.ToString("yyyy-MM-dd"));

        //InstructorID is an IHtmlSelectElement. For some reason the value is parsed as NULL by AngleSharp even when it is filled in so we can't check this field
    }

    [Fact]
    public async Task DepartmentEdit_WhenCancelButtonClicked_PopupCloses()
    {
        var departmentDetailVM = _fixture.Create<DepartmentDetailVM>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("button[type='button']").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task DepartmentEdit_WhenEditButtonClicked_PopupCloses()
    {
        var departmentDetailVM = _fixture.Create<DepartmentDetailVM>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentEdit>(title, parameters));
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
    public async Task DepartmentEdit_WhenEditButtonClicked_DepartmentServiceMustBeCalled()
    {
        var departmentDetailVM = _fixture.Create<DepartmentDetailVM>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#Name").Change("My name");
        comp.Find("#Budget").Change("123");
        comp.Find("#StartDate").Change("1/3/2021");
        comp.Find("#InstructorID").Change("1");

        comp.Find("button[type='submit']").Click();

        A.CallTo(() => fakeDepartmentService.UpdateAsync(A<UpdateDepartmentDto>.That.IsInstanceOf(typeof(UpdateDepartmentDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task DepartmentEdit_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var departmentDetailVM = _fixture.Create<DepartmentDetailVM>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailVM);
        A.CallTo(() => fakeDepartmentService.UpdateAsync(A<UpdateDepartmentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as DepartmentEdit;

        comp.Find("#Name").Change("My name");
        comp.Find("#Budget").Change("123");
        comp.Find("#StartDate").Change("1/3/2021");
        comp.Find("#InstructorID").Change("1");

        comp.Find("button[type='submit']").Click();

        dialog?.ErrorVisible.Should().Be(true);
        comp.Find("div.mud-alert-message").TrimmedText().Should().Be("An error occured during saving");
    }

    [Fact]
    public async Task DepartmentEdit_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var departmentDetailVM = _fixture.Create<DepartmentDetailVM>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(() => dialogReference = service?.Show<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#Name").Change("");
        comp.Find("#Budget").Change("");
        comp.Find("#StartDate").Change("");
        comp.Find("#InstructorID").Change("");

        comp.Find("button[type='submit']").Click();

        comp.FindAll("div.validation-message")[0].TrimmedText().Should().Be("'Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().Should().Be("The Budget field must be a number.");
        comp.FindAll("div.validation-message")[2].TrimmedText().Should().Be("The StartDate field must be a date.");
    }

    private static InstructorsLookupVM GetInstructorsLookupVMWithTestData()
    {
        return new InstructorsLookupVM(new List<InstructorLookupVM>
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
