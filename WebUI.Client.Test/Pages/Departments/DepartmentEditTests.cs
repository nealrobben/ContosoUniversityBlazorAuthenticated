using AngleSharp.Html.Dom;
using AutoFixture;
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

public class DepartmentEditTests : BunitTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task DepartmentEdit_DisplayDialogCorrectly()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().ShouldBe(title);

        ((IHtmlInputElement)comp.FindAll("input")[0]).Value.ShouldBe(departmentDetailDto.Name);
        ((IHtmlInputElement)comp.FindAll("input")[1]).Value.ShouldBe(departmentDetailDto.Budget.ToString());
        ((IHtmlInputElement)comp.FindAll("input")[2]).Value.ShouldBe(departmentDetailDto.StartDate.ToString("yyyy-MM-dd"));

        //InstructorID is an IHtmlSelectElement. For some reason the value is parsed as NULL by AngleSharp even when it is filled in so we can't check this field
    }

    [Fact]
    public async Task DepartmentEdit_WhenCancelButtonClicked_PopupCloses()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button[type='button']").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }

    [Fact]
    public async Task DepartmentEdit_WhenEditButtonClicked_PopupCloses()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentEdit>(title, parameters));
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
    public async Task DepartmentEdit_WhenEditButtonClicked_DepartmentServiceMustBeCalled()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#Name").ChangeAsync("My name");
        await comp.Find("#Budget").ChangeAsync("123");
        await comp.Find("#StartDate").ChangeAsync("1/3/2021");
        await comp.Find("#InstructorID").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();

        A.CallTo(() => fakeDepartmentService.UpdateAsync(A<UpdateDepartmentDto>.That.IsInstanceOf(typeof(UpdateDepartmentDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task DepartmentEdit_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        A.CallTo(() => fakeDepartmentService.UpdateAsync(A<UpdateDepartmentDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as DepartmentEdit;

        await comp.Find("#Name").ChangeAsync("My name");
        await comp.Find("#Budget").ChangeAsync("123");
        await comp.Find("#StartDate").ChangeAsync("1/3/2021");
        await comp.Find("#InstructorID").ChangeAsync("1");

        await comp.Find("button[type='submit']").ClickAsync();

        dialog?.ErrorVisible.ShouldBe(true);
        comp.Find("div.mud-alert-message").TrimmedText().ShouldBe("An error occured during saving");
    }

    [Fact]
    public async Task DepartmentEdit_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(GetInstructorsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Edit Department";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#Name").ChangeAsync("");
        await comp.Find("#Budget").ChangeAsync("");
        await comp.Find("#StartDate").ChangeAsync("");
        await comp.Find("#InstructorID").ChangeAsync("");

        await comp.Find("button[type='submit']").ClickAsync();

        comp.FindAll("div.validation-message")[0].TrimmedText().ShouldBe("'Name' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().ShouldBe("The Budget field must be a number.");
        comp.FindAll("div.validation-message")[2].TrimmedText().ShouldBe("The StartDate field must be a date.");
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