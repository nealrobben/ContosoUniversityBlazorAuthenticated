
using Bunit;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using WebUI.Client.InputModels.Courses;
using WebUI.Client.Shared;

namespace WebUI.Client.Test;

public abstract class BunitTestBase
{
    private bool _mudPopoverAdded;
    protected TestContext Context { get; private set; }

    protected BunitTestBase()
    {
        Context = new TestContext();

        Context.JSInterop.Mode = JSRuntimeMode.Loose;

        Context.Services.AddMudServices(options =>
        {
            options.SnackbarConfiguration.ShowTransitionDuration = 0;
            options.SnackbarConfiguration.HideTransitionDuration = 0;
        });

        Context.Services.AddLocalization(opts => { opts.ResourcesPath = "Localization"; });

        Context.Services.AddValidatorsFromAssemblyContaining<CreateCourseInputModel>();
    }

    private IRenderedComponent<IComponent> RenderWithMudPopover<TComponent>()
        where TComponent : IComponent
    {
        return Context.RenderComponent<UnitTestLayout>(hostParams => hostParams
        .AddChildContent<TComponent>());
    }

    private IRenderedComponent<IComponent> RenderWithoutMudPopover<TComponent>()
        where TComponent : IComponent
    {
        return (IRenderedComponent<IComponent>)Context.RenderComponent<TComponent>();
    }

    protected IRenderedComponent<IComponent> RenderComponent<TComponent>()
        where TComponent : IComponent
    {
        if (_mudPopoverAdded)
        {
            return RenderWithoutMudPopover<TComponent>();
        }
        else
        {
            _mudPopoverAdded = true;
            return RenderWithMudPopover<TComponent>();
        }
    }
}
