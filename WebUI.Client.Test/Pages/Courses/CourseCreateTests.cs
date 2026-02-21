using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Courses;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Pages.Courses;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using Xunit;

namespace WebUI.Client.Test.Pages.Courses;

public class CourseCreateTests : BunitTestBase
{
    [Fact]
    public async Task CourseCreate_DisplayDialogCorrectly()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Course";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<CourseCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().Should().Be(title);

        comp.FindAll("input")[0].Id.Should().Be("CourseID");
        comp.FindAll("input")[1].Id.Should().Be("Title");
        comp.FindAll("input")[2].Id.Should().Be("Credits");
        comp.FindAll("input")[3].Id.Should().Be("Department");
    }

    [Fact]
    public async Task CourseCreate_WhenCancelButtonClicked_PopupCloses()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Course";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<CourseCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button[type='button']").ClickAsync();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task CourseCreate_WhenCreateButtonClicked_PopupCloses()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Course";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<CourseCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#CourseID").ChangeAsync("1");
        await comp.Find("#Title").ChangeAsync("My title");
        await comp.Find("#Credits").ChangeAsync("2");
        await comp.Find("#Department").ChangeAsync("3");

        await comp.Find("button[type='submit']").ClickAsync();
        comp.Markup.Trim().Should().BeEmpty();
    }

    [Fact]
    public async Task CourseCreate_WhenCreateButtonClicked_CourseServiceMustBeCalled()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Course";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<CourseCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("#CourseID").ChangeAsync("1");
        await comp.Find("#Title").ChangeAsync("My title");
        await comp.Find("#Credits").ChangeAsync("2");
        await comp.Find("#Department").ChangeAsync("3");

        await comp.Find("button[type='submit']").ClickAsync();

        A.CallTo(() => fakeCourseService.CreateAsync(A<CreateCourseDto>.That.IsInstanceOf(typeof(CreateCourseDto)))).MustHaveHappened();
    }

    [Fact]
    public async Task CourseCreate_WhenExceptionCaughtAfterSave_ShowErrorMessage()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.CreateAsync(A<CreateCourseDto>.Ignored)).ThrowsAsync(new Exception("error"));
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Course";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<CourseCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        var dialog = dialogReference?.Dialog as CourseCreate;

        await comp.Find("#CourseID").ChangeAsync("1");
        await comp.Find("#Title").ChangeAsync("My title");
        await comp.Find("#Credits").ChangeAsync("2");
        await comp.Find("#Department").ChangeAsync("3");

        await comp.Find("button[type='submit']").ClickAsync();

        dialog?.ErrorVisible.Should().Be(true);
        comp.Find("div.mud-alert-message").TrimmedText().Should().Be("An error occured during saving");
    }

    [Fact]
    public async Task CourseCreate_WhenValidationFails_ShowErrorMessagesForFields()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetLookupAsync()).Returns(GetDepartmentsLookupDtoWithTestData());
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeCourseService = A.Fake<ICourseService>();
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        const string title = "Create Course";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<CourseCreate>(title));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button[type='submit']").ClickAsync();

        comp.FindAll("div.validation-message")[0].TrimmedText().Should().Be("'Course ID' must not be empty.");
        comp.FindAll("div.validation-message")[1].TrimmedText().Should().Be("'Title' must not be empty.");
        comp.FindAll("div.validation-message")[2].TrimmedText().Should().Be("'Credits' must not be empty.");
        comp.FindAll("div.validation-message")[3].TrimmedText().Should().Be("'Credits' must be greater than '0'.");
    }

    private static DepartmentsLookupDto GetDepartmentsLookupDtoWithTestData()
    {
        return new DepartmentsLookupDto(new List<DepartmentLookupDto>
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
