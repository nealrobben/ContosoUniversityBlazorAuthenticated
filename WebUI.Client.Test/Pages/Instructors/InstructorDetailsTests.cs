﻿
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Pages.Instructors;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using WebUI.Shared.Instructors.Queries.GetInstructorDetails;
using Xunit;

namespace WebUI.Client.Test.Pages.Instructors;

public class InstructorDetailsTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public InstructorDetailsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task InstructorDetails_DisplayDetailsCorrectly()
    {
        var instructorDetailsVM = _fixture.Create<InstructorDetailsVM>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsVM);
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        var title = "Instructor Details";
        await comp.InvokeAsync(() => dialogReference = service?.Show<InstructorDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().Should().Be(title);

        comp.FindAll("dt")[0].TrimmedText().Should().Be("Last name");
        comp.FindAll("dt")[1].TrimmedText().Should().Be("First name");
        comp.FindAll("dt")[2].TrimmedText().Should().Be("Hire date");

        comp.FindAll("dd")[0].TrimmedText().Should().Be(instructorDetailsVM.LastName);
        comp.FindAll("dd")[1].TrimmedText().Should().Be(instructorDetailsVM.FirstName);
        comp.FindAll("dd")[2].TrimmedText().Should().Be(instructorDetailsVM.HireDate.ToShortDateString());
    }

    [Fact]
    public async Task InstructorDetails_WhenOkButtonClicked_PopupCloses()
    {
        var instructorDetailsVM = _fixture.Create<InstructorDetailsVM>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetAsync(A<string>.Ignored)).Returns(instructorDetailsVM);
        Context.Services.AddScoped(x => fakeInstructorService);

        var comp = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "InstructorId", 1 }
        };

        var title = "Instructor Details";
        await comp.InvokeAsync(() => dialogReference = service?.Show<InstructorDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("button").Click();
        comp.Markup.Trim().Should().BeEmpty();
    }
}
