using AutoMapper;
using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Core.Authentication.Users;

namespace Dvchevskii.Blog.Application.Authentication.Users;

public class UsersMapperProfile : Profile
{
    public UsersMapperProfile()
    {
        CreateMap<SignUpDto, CreateUserDto>()
            .ForMember(dest => dest.PrimaryEmail, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.IsAdmin, opt => opt.Ignore());
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AuditInfo, opt => opt.Ignore());
        CreateMap<User, UserDto>();
    }
}
