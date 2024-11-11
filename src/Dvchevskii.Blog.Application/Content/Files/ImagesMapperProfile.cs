using AutoMapper;
using Dvchevskii.Blog.Contracts.Files;
using Dvchevskii.Blog.Core.Files;

namespace Dvchevskii.Blog.Application.Content.Files;

public class ImagesMapperProfile : Profile
{
    public ImagesMapperProfile()
    {
        CreateMap<Image, ImageDto>();
    }
}
