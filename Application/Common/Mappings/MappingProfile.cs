namespace ContosoUniversityBlazor.Application.Common.Mappings;

using AutoMapper;
using global::System;
using global::System.Linq;
using global::System.Reflection;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => Array.Exists(t.GetInterfaces(),
                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping") 
                ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");
            
            methodInfo?.Invoke(instance, new object[] { this });

        }
    }
}