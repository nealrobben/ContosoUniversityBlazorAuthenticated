using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WebUINew.Client.InputModels.Courses;
using WebUINew.Client.Services;

namespace WebUINew.Client;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddMudServices();

        RegisterServices(builder);

        builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Localization"; });

        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseInputModel>();

        var host = builder.Build();

        await SetCulture(host);

        await host.RunAsync();
    }

    private static void RegisterServices(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IInstructorService, InstructorService>();
        builder.Services.AddScoped<IStudentService, StudentService>();
        builder.Services.AddScoped<IFileUploadService, FileUploadService>();
        builder.Services.AddScoped<ClientSettingService>();
    }

    private static async Task SetCulture(WebAssemblyHost host)
    {
        var clientSettingService = host.Services.GetRequiredService<ClientSettingService>();
        if (clientSettingService != null)
        {
            var preference = await clientSettingService.GetSettings();
            var culture = preference != null ? new CultureInfo(preference.LanguageCode) : new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}