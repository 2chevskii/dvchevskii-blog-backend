using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dvchevskii.Blog.Contracts.Files;
using Dvchevskii.Blog.Core.Files;
using Dvchevskii.Blog.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Blog.Application.Content.Files;

public class ImageService(
    BlogDbContext dbContext,
    IConfigurationProvider mapperConfigurationProvider,
    IMapper mapper,
    ImageStorage imageStorage
) : IImageService
{
    public async Task<ImageDto> Get(int id)
    {
        var imageDto = await dbContext.Images.ProjectTo<ImageDto>(mapperConfigurationProvider)
            .FirstAsync(x => x.Id == id);

        return imageDto;
    }

    public Task<bool> Exists(int id)
    {
        return dbContext.Images.AnyAsync(x => x.Id == id);
    }


    public async Task<ImageDto> Upload(Stream contentStream, string filename)
    {
        var fileExtension = Path.GetExtension(filename);
        var storageFilename = $"{Guid.NewGuid():N}{fileExtension}";

        var image = new Image
        {
            Filename = storageFilename,
        };

        imageStorage.Save(image.Filename, contentStream);
        dbContext.Images.Add(image);
        await dbContext.SaveChangesAsync();

        return mapper.Map<ImageDto>(image);
    }

    public async Task<Stream> Download(int id)
    {
        var image = await dbContext.Images.ProjectTo<ImageDto>(mapperConfigurationProvider).FirstAsync(x => x.Id == id);

        var imageContent = imageStorage.Read(image.Filename);

        return imageContent;
    }
}
