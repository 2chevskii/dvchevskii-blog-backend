using AutoMapper;
using Dvchevskii.Blog.Contracts.Posts.Entities;
using Dvchevskii.Blog.Core.Content.Posts;

namespace Dvchevskii.Blog.Application.Posts;

public class PostsMapperProfile : Profile
{
    public PostsMapperProfile()
    {
        CreateMap<Post, PostDto>();
        CreateMap<Post, PostDisplayInfoDto>()
            .ForMember(
                dest => dest.ModifiedAtUtc,
                opt => opt.MapFrom(src =>
                    src.AuditInfo.UpdatedAtUtc ?? src.AuditInfo.CreatedAtUtc
                )
            );
        CreateMap<PostDto, Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AuditInfo, opt => opt.Ignore())
            .ForMember(dest => dest.HeaderImage, opt => opt.Ignore());
    }
}
