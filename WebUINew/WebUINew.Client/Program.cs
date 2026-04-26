using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace WebUINew.Client;

[ExcludeFromCodeCoverage]
internal static class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddMudServices();

        RegisterServices(builder);

        builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Localization"; });

        builder.Services.AddBlazoredLocalStorage();
        //builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseInputModel>(); //TODO

        var host = builder.Build();

        await SetCulture(host);

        await host.RunAsync();
    }

    private static void RegisterServices(WebAssemblyHostBuilder builder)
    {
        //builder.Services.AddScoped<IDepartmentService, DepartmentService>();//TODO
        //builder.Services.AddScoped<ICourseService, CourseService>();//TODO
        //builder.Services.AddScoped<IInstructorService, InstructorService>();//TODO
        //builder.Services.AddScoped<IStudentService, StudentService>();//TODO
        //builder.Services.AddScoped<IFileUploadService, FileUploadService>();//TODO
        //builder.Services.AddScoped<ClientSettingService>();//TODO
    }

    private static async Task SetCulture(WebAssemblyHost host)
    {
        //var clientSettingService = host.Services.GetRequiredService<ClientSettingService>(); //TODO
        //if (clientSettingService != null)
        //{
        //    var preference = await clientSettingService.GetSettings();
        //    var culture = preference != null ? new CultureInfo(preference.LanguageCode) : new CultureInfo("en-US");
        //    CultureInfo.DefaultThreadCurrentCulture = culture;
        //    CultureInfo.DefaultThreadCurrentUICulture = culture;
        //}
    }
}