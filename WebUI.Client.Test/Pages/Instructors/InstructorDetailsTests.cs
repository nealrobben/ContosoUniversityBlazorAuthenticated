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

        comp.Find("h6").TrimmedText().ShouldBe(title);

        comp.FindAll("dt")[0].TrimmedText().ShouldBe("Last name");
        comp.FindAll("dt")[1].TrimmedText().ShouldBe("First name");
        comp.FindAll("dt")[2].TrimmedText().ShouldBe("Hire date");

        comp.FindAll("dd")[0].TrimmedText().ShouldBe(instructorDetailsDto.LastName);
        comp.FindAll("dd")[1].TrimmedText().ShouldBe(instructorDetailsDto.FirstName);
        comp.FindAll("dd")[2].TrimmedText().ShouldBe(instructorDetailsDto.HireDate.ToShortDateString());
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
        comp.Markup.Trim().ShouldBeEmpty();
    }
}