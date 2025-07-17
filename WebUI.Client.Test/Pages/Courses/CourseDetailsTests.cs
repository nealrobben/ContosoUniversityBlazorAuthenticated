
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Courses;
using WebUI.Client.Pages.Courses;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using Xunit;

namespace WebUI.Client.Test.Pages.Courses;

public class CourseDetailsTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public CourseDetailsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CourseDetails_DisplayDetailsCorrectly()
    {
        var courseDetailsDto = _fixture.Create<CourseDetailDto>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsDto);
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Course Details";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<CourseDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().Should().Be(title);

        comp.FindAll("dt")[0].TrimmedText().Should().Be("Title");
        comp.FindAll("dt")[1].TrimmedText().Should().Be("Credits");
        comp.FindAll("dt")[2].TrimmedText().Should().Be("Department");

        comp.FindAll("dd")[0].TrimmedText().Should().Be(courseDetailsDto.Title);
        comp.FindAll("dd")[1].TrimmedText().Should().Be(courseDetailsDto.Credits.ToString());
        comp.FindAll("dd")[2].TrimmedText().Should().Be(courseDetailsDto.DepartmentID.ToString());
    }

    [Fact]
    public async Task CourseDetails_WhenOkButtonClicked_PopupCloses()
    {
        var courseDetailsDto = _fixture.Create<CourseDetailDto>();

        var fakeCourseService = A.Fake<ICourseService>();
        A.CallTo(() => fakeCourseService.GetAsync(A<string>.Ignored)).Returns(courseDetailsDto);
        Context.Services.AddScoped(x => fakeCourseService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "CourseId", 1 }
        };

        const string title = "Course Details";
        await comp.InvokeAsync(async () => dialogReference = await service!.ShowAsync<CourseDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("button").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }
}
