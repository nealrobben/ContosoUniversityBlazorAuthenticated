using AutoFixture;
using Bunit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Shouldly;
using WebUI.Client.Dtos.Departments;
using WebUI.Client.Pages.Departments;
using WebUI.Client.Services;
using WebUI.Client.Tests.Extensions;

namespace WebUI.Client.Tests.Pages.Departments;

public class DepartmentDetailsTests : BunitTestBase
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task DepartmentDetails_DisplayDetailsCorrectly()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Department Details";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h6").TrimmedText().ShouldBe(title);

        comp.FindAll("dt")[0].TrimmedText().ShouldBe("Name");
        comp.FindAll("dt")[1].TrimmedText().ShouldBe("Budget");
        comp.FindAll("dt")[2].TrimmedText().ShouldBe("Start date");
        comp.FindAll("dt")[3].TrimmedText().ShouldBe("Administrator");

        comp.FindAll("dd")[0].TrimmedText().ShouldBe(departmentDetailDto.Name);
        comp.FindAll("dd")[1].TrimmedText().ShouldBe(departmentDetailDto.Budget.ToString("F"));
        comp.FindAll("dd")[2].TrimmedText().ShouldBe(departmentDetailDto.StartDate.ToShortDateString());
        comp.FindAll("dd")[3].TrimmedText().ShouldBe(departmentDetailDto.AdministratorName);
    }

    [Fact]
    public async Task DepartmentDetails_WhenOkButtonClicked_PopupCloses()
    {
        var departmentDetailDto = _fixture.Create<DepartmentDetailDto>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAsync(A<string>.Ignored)).Returns(departmentDetailDto);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.Render<MudDialogProvider>();
        Assert.Empty(comp.Markup.Trim());

        var service = Context.Services.GetService<IDialogService>() as DialogService;
        Assert.NotNull(service);
        IDialogReference? dialogReference = null;

        var parameters = new DialogParameters
        {
            { "DepartmentId", 1 }
        };

        const string title = "Department Details";
        await comp.InvokeAsync(async () => dialogReference = await service.ShowAsync<DepartmentDetails>(title, parameters));
        Assert.NotNull(dialogReference);

        Assert.NotEmpty(comp.Markup.Trim());

        await comp.Find("button").ClickAsync();
        comp.Markup.Trim().ShouldBeEmpty();
    }
}