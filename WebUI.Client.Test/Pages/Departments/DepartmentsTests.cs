
using AutoFixture;
using Bunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using WebUI.Client.Dtos.Instructors;
using WebUI.Client.Services;
using WebUI.Client.Test.Extensions;
using WebUI.Shared.Common;
using WebUI.Shared.Departments.Queries.GetDepartmentsOverview;
using Xunit;

namespace WebUI.Client.Test.Pages.Departments;

public class DepartmentsTests : BunitTestBase
{
    private readonly Fixture _fixture;

    public DepartmentsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Departments_ClickCreateButton_OpensDialog()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fixture = new Fixture();
        var instructorsLookupDto = fixture.Create<InstructorsLookupDto>();

        var fakeInstructorService = A.Fake<IInstructorService>();
        A.CallTo(() => fakeInstructorService.GetLookupAsync()).Returns(instructorsLookupDto);
        Context.Services.AddScoped(x => fakeInstructorService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Departments.Departments>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("h2").TrimmedText().Should().Be("Departments");
        comp.Find("#CreateButton").Should().NotBeNull();

        comp.Find("#CreateButton").Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Departments_ClickSearch_CallsDepartmentService()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<Client.Pages.Departments.Departments>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#SearchButton").Should().NotBeNull();
        comp.Find("#SearchButton").Click();

        A.CallTo(() => fakeDepartmentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Departments_ClickBackToFullList_CallsDepartmentService()
    {
        var fakeDepartmentService = A.Fake<IDepartmentService>();
        Context.Services.AddScoped(x => fakeDepartmentService);

        var comp = Context.RenderComponent<Client.Pages.Departments.Departments>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.Find("#BackToFullListButton").Should().NotBeNull();
        comp.Find("#BackToFullListButton").Click();

        A.CallTo(() => fakeDepartmentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public void Departments_ClickDetailsButton_OpensDialog()
    {
        var departmentsOverviewVM = _fixture.Create<OverviewVM<DepartmentVM>>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(departmentsOverviewVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Departments.Departments>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenDepartmentDetailsButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenDepartmentDetailsButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Departments_ClickEditButton_OpensDialog()
    {
        var departmentsOverviewVM = _fixture.Create<OverviewVM<DepartmentVM>>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(departmentsOverviewVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        Context.Services.AddScoped(x => fakeInstructorService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Departments.Departments>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".OpenDepartmentEditButton")[0].Should().NotBeNull();
        comp.FindAll(".OpenDepartmentEditButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());
    }

    [Fact]
    public void Departments_ClickDeleteButton_ShowsConfirmationDialog()
    {
        var departmentsOverviewVM = _fixture.Create<OverviewVM<DepartmentVM>>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(departmentsOverviewVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        Context.Services.AddScoped(x => fakeInstructorService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Departments.Departments>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".DepartmentDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".DepartmentDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.Find(".mud-dialog-content").TrimmedText().Should().Be($"Are you sure you want to delete the department {departmentsOverviewVM.Records[0].Name}?");
    }

    [Fact]
    public void Departments_ClickDeleteButtonAndConfirm_DepartmentServiceShouldBeCalled()
    {
        var departmentsOverviewVM = _fixture.Create<OverviewVM<DepartmentVM>>();

        var fakeDepartmentService = A.Fake<IDepartmentService>();
        A.CallTo(() => fakeDepartmentService.GetAllAsync(A<string>.Ignored, A<int?>.Ignored, A<string>.Ignored, A<int?>.Ignored)).Returns(departmentsOverviewVM);
        Context.Services.AddScoped(x => fakeDepartmentService);

        var fakeInstructorService = A.Fake<IInstructorService>();
        Context.Services.AddScoped(x => fakeInstructorService);

        var dialog = Context.RenderComponent<MudDialogProvider>();
        Assert.Empty(dialog.Markup.Trim());

        var comp = Context.RenderComponent<Client.Pages.Departments.Departments>();
        Assert.NotEmpty(comp.Markup.Trim());

        comp.FindAll(".DepartmentDeleteButton")[0].Should().NotBeNull();
        comp.FindAll(".DepartmentDeleteButton")[0].Click();

        Assert.NotEmpty(dialog.Markup.Trim());

        dialog.FindAll("button")[1].Should().NotBeNull();
        dialog.FindAll("button")[1].Click();

        A.CallTo(() => fakeDepartmentService.DeleteAsync(A<string>.Ignored)).MustHaveHappened();
    }
}
