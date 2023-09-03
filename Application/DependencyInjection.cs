namespace ContosoUniversityBlazor.Application;

using ContosoUniversityBlazor.Application.Common.Behaviours;
using FluentValidation;
using global::System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Removed because we use the one in WebUI.Shared and automapper only accepts one assembly
        //services.AddAutoMapper(Assembly.GetExecutingAssembly()); //NOSONAR

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

        return services;
    }
}
