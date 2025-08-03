using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.Common.Interfaces;
using Application.Program.Commands.SeedData;
using Infrastructure;
using Infrastructure.Persistence;
using WebUI.Server.Filters;
using WebUI.Server.Services;

namespace WebUI.Server;

#pragma warning disable S1118 // Utility classes should not have public constructors
[ExcludeFromCodeCoverage]
public class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews(options =>options.Filters.Add(new ApiExceptionFilterAttribute()));
        builder.Services.AddRazorPages();
        builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<SchoolContext>();

        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddSwaggerDocument();

        var app = builder.Build();

        if (!app.Configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            await MigrateDatabase(app.Services);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();

            //Do NOT use this method with .Net 5, it will give an exception when processing a request
            //app.UseDatabaseErrorPage(); //NOSONAR
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHealthChecks("/health");

        app.UseStaticFiles();

        app.UseOpenApi();
        app.UseSwaggerUi(settings =>
        {
        });

        app.UseRouting();

        app.UseAntiforgery();

        app.MapRazorPages();
        app.MapControllers();
        app.MapRazorComponents<WebUI.Client.Host>().AddInteractiveWebAssemblyRenderMode();

        try
        {
            Log.Information("Application Starting.");
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The Application failed to start.");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static async Task MigrateDatabase(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        try
        {
            var context = serviceProvider.GetRequiredService<SchoolContext>();

            await context.Database.EnsureCreatedAsync();

            var mediator = serviceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SeedDataCommand(), CancellationToken.None);
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
    }
}
