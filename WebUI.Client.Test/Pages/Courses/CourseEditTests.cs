
using AngleSharp.Html.Dom;
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Pages.Courses;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using WebUI.Shared.Courses.Commands.UpdateCourse;
using WebUI.Shared.Courses.Queries.GetCourseDetails;
using WebUI.Shared.Departments.Queries.GetDepartmentsLookup;
using Xunit;

namespace WebUI.Client.Test.Pages.Courses;

public class CourseEditTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public CourseEditTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CourseEdit_DisplayDialogCorrectly()
    {
        var courseDetailsVM = _fixture.Create<CourseDetailVM>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Edit Course";
        await comp.InvokeAsync(() => dialogReference = service?.Show<CourseEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        ((IHtmlInputElement)comp.FindAll("input")[0]).Value.Should().Be(courseDetailsVM.Title);
        ((IHtmlInputElement)comp.FindAll("input")[1]).Value.Should().Be(courseDetailsVM.Credits.ToString());

        //DepartmentID is an IHtmlSelectElement. For some reason the value is parsed as NULL by AngleSharp even when it is filled in so we can't check this field
    }

    [Fact]
    public async Task CourseEdit_WhenCancelButtonClicked_PopupCloses()
    {
        var courseDetailsVM = _fixture.Create<CourseDetailVM>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Edit Course";
        await comp.InvokeAsync(() => dialogReference = service?.Show<CourseEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("button[type='button']").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task CourseEdit_WhenEditButtonClicked_PopupCloses()
    {
        var courseDetailsVM = _fixture.Create<CourseDetailVM>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Edit Course";
        await comp.InvokeAsync(() => dialogReference = service?.Show<CourseEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#Title").Change("My title");
        comp.Find("#Credits").Change("123");
        comp.Find("#Department").Change("1");

        comp.Find("button[type='submit']").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task CourseEdit_WhenEditButtonClicked_CourseServiceMustBeCalled()
    {
        var courseDetailsVM = _fixture.Create<CourseDetailVM>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Edit Course";
        await comp.InvokeAsync(() => dialogReference = service?.Show<CourseEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#Title").Change("My title");
        comp.Find("#Credits").Change("123");
        comp.Find("#Department").Change("1");

        comp.Find("button[type='submit']").Click();

        A.CallTo(() => fakeCourseService.UpdateAsync(A<UpdateCourseCommand>.That.IsInstanceOf(typeof(UpdateCourseCommand)))).MustHaveHappened();
    }

    [Fact]
    public async Task CourseEdit_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var courseDetailsVM = _fixture.Create<CourseDetailVM>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsVM);
        A.CallTo(() => fakeCourseService.UpdateAsync(A<UpdateCourseCommand>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Edit Course";
        await comp.InvokeAsync(() => dialogReference = service?.Show<CourseEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as CourseEdit;

        comp.Find("#Title").Change("My title");
        comp.Find("#Credits").Change("123");
        comp.Find("#Department").Change("1");

        comp.Find("button[type='submit']").Click();

        dialog?.ErrorVisible.Should().Be(true);
        comp.Find("div.mud-alert-message").TrimmedText().Should().Be("An error occured during saving");
    }

    [Fact]
    public async Task CourseEdit_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var courseDetailsVM = _fixture.Create<CourseDetailVM>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsVM);
        Context.Services.AddScoped(x => fakeCourseService);

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupVMWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Edit Course";
        await comp.InvokeAsync(() => dialogReference = service?.Show<CourseEdit>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#Title").Change("");
        comp.Find("#Credits").Change("");
        comp.Find("#Department").Change("");

        comp.Find("button[type='submit']").Click();

        comp.FindAll("div.validation-message")[0].TrimmedText().Should().Be("'Title' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().Should().Be("The Credits field must be a number.");
    }

    private static DepartmentsLookupVM GetDepartmentsLookupVMWithTestData()
    {
        return new DepartmentsLookupVM(new List<DepartmentLookupVM>
        {
            new() {
                DepartmentID = 1,
                Name = "Department One"
            },
            new() {
                DepartmentID = 2,
                Name = "Department Two"
            },
            new() {
                DepartmentID = 3,
                Name = "Department Three"
            }
        });
    }
}
