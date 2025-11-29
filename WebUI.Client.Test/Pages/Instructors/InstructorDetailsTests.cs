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

public class InstructorDetailsTests : BunitTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task InstructorDetails_DisplayDetailsCorrectly()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Instructor Details";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().Should().Be(title);

        comp.FindAll("dt")[0].TrimmedText().Should().Be("Last name");
        comp.FindAll("dt")[1].TrimmedText().Should().Be("First name");
        comp.FindAll("dt")[2].TrimmedText().Should().Be("Hire date");

        comp.FindAll("dd")[0].TrimmedText().Should().Be(instructorDetailsDto.LastName);
        comp.FindAll("dd")[1].TrimmedText().Should().Be(instructorDetailsDto.FirstName);
        comp.FindAll("dd")[2].TrimmedText().Should().Be(instructorDetailsDto.HireDate.ToShortDateString());
    }

    [Fact]
    public async Task InstructorDetails_WhenOkButtonClicked_PopupCloses()
    {
        var instructorDetailsDto = _fixture.Create<InstructorDetailDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        const string title = "Instructor Details";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<InstructorDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button").ClickAsync();
        comp.Markup.Trim().Should().BeEmpty();
    }
}
