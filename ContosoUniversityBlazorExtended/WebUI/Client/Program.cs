using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Client.ViewModels.Courses;
using WebUI.Client.ViewModels.Instructors;
using WebUI.Client.ViewModels.Students;
using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Blazored.LocalStorage;

namespace WebUI.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices();

            RegisterServices(builder);
            RegisterViewModels(builder);

            builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Localization"; });

            builder.Services.AddBlazoredLocalStorage();

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
            builder.Services.AddScoped<FileuploadService>();
            builder.Services.AddScoped<ClientSettingService>();
        }

        private static void RegisterViewModels(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddTransient<CourseCreateViewModel>();
            builder.Services.AddTransient<CourseDetailsViewModel>();
            builder.Services.AddTransient<CoursesViewModel>();
            builder.Services.AddTransient<CourseEditViewModel>();

            builder.Services.AddTransient<InstructorCreateViewModel>();
            builder.Services.AddTransient<InstructorDetailsViewModel>();
            builder.Services.AddTransient<InstructorsViewModel>();
            builder.Services.AddTransient<InstructorEditViewModel>();

            builder.Services.AddTransient<StudentCreateViewModel>();
            builder.Services.AddTransient<StudentDetailsViewModel>();
            builder.Services.AddTransient<StudentsViewModel>();
            builder.Services.AddTransient<StudentEditViewModel>();
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
}
