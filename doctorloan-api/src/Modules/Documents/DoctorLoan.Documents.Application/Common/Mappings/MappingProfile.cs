using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Contents.Application.Features.Contents.Admin.Commands;
using DoctorLoan.Domain.Entities.Contents;
using AutoMapper;

namespace DoctorLoan.Contents.Application.Common.Mappings;
public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        CreateMap<CreateContentCommand, Content>();
    }

    public int Order => 0;
}
