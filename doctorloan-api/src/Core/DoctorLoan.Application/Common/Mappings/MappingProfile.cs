using System.Reflection;
using AutoMapper;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Medias;
using DoctorLoan.Domain.Entities.Medias;

namespace DoctorLoan.Application.Common.Mappings;

public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        CreateMap<Media, MediaInfo>();
    }

    public int Order => 0;

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });

        }
    }
}
