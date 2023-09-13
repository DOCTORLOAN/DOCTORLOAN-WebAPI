using System.Reflection;
using AutoMapper;
using DoctorLoan.Application.Common.Mappings;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.User.Application.Features.Users;
using DoctorLoan.User.Application.Features.Users.Dtos;

namespace DoctorLoan.User.Application.Mappings;

public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        CreateMap<AddUserCommand, Domain.Entities.Users.User>()
            .ForMember(s => s.Id, otps => otps.Ignore());

        _ = CreateMap<Domain.Entities.Users.User, UserDto>()
            .ForMember(s => s.RoleName, otps => otps.MapFrom(fo => fo.Role.Name))
            .ForMember(s => s.LastSignIn, otps => otps.MapFrom(fo => !fo.UserActivities.Any() ? (DateTimeOffset?)null : fo.UserActivities.OrderByDescending(s => s.Id).FirstOrDefault().Created));

        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
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
