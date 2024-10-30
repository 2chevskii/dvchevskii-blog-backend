using AutoMapper;
using Dvchevskii.Blog.Contracts.Common;
using Dvchevskii.Blog.Core.Common;

namespace Dvchevskii.Blog.Application.Common;

public class EntityBaseMapperProfile : Profile
{
    public EntityBaseMapperProfile()
    {
        CreateMap<EntityBase, EntityBaseDto>();
    }
}
