
using Bunit;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using WebUI.Client.InputModels.Courses;
using WebUI.Shared.Common;

namespace WebUI.Client.Test;

public abstract class BunitTestBase
{
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

        Context.Services.AddValidatorsFromAssemblyContaining<MetaData>(); //TODO: Remove after shared project has been removed
        Context.Services.AddValidatorsFromAssemblyContaining<CreateCourseInputModel>();
    }
}
