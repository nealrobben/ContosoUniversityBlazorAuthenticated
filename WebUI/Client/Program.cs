
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebUI.Client.Services;
using System.Globalization;
using Blazored.LocalStorage;
using FluentValidation;
using WebUI.Client.InputModels.Courses;

namespace WebUI.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("app");

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
            CultureInfo culture;
            var preference = await clientSettingService.GetSettings();
            if (preference != null)
                culture = new CultureInfo(preference.LanguageCode);
            else
                culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
