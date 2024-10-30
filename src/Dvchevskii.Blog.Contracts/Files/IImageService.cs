namespace Dvchevskii.Blog.Contracts.Files;

public interface IImageService
{
    Task<ImageDto> Get(int id);
    Task<ImageDto> Upload(Stream contentStream, string filename);
    Task<Stream> Download(int id);
}
