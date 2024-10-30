using AutoMapper;
using Dvchevskii.Blog.Contracts.Authentication;

namespace Dvchevskii.Blog.WebApi.Auth;

public record SignUpRequest(string Username, string Password, string? Email);

public class AuthRequestsMapperProfile : Profile
{
    public AuthRequestsMapperProfile()
    {
        CreateMap<SignUpRequest, SignUpDto>();
        CreateMap<SignInWithLoginPasswordRequest, LoginPasswordDto>();
    }
}
