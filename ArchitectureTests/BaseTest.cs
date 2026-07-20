using Application.Common.Interfaces;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Domain.Common;
using Infrastructure.Services;
using WebUI.Client;
using WebUI.Server.Controllers;
using Assembly = System.Reflection.Assembly;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(AuditableEntity).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ICurrentUserService).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(DateTimeService).Assembly;
    protected static readonly Assembly WebClientAssembly = typeof(App).Assembly;
    protected static readonly Assembly WebServerAssembly = typeof(AboutController).Assembly;

    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            DomainAssembly,
            ApplicationAssembly,
            InfrastructureAssembly,
            WebClientAssembly,
            WebServerAssembly)
        .Build();
}