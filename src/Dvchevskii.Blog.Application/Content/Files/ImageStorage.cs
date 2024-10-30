using Microsoft.Extensions.Options;

namespace Dvchevskii.Blog.Application.Content.Files;

public class ImageStorage(IOptions<ImageStorageOptions> options)
{
    public bool Exists(string filename)
    {
        return File.Exists(GetFullPath(filename));
    }

    public void Save(string filename, Stream contentStream)
    {
        var fullPath = GetFullPath(filename);
        if (Exists(fullPath))
        {
            throw new Exception("File exists");
        }

        using var writeStream = File.OpenWrite(fullPath);
        contentStream.CopyTo(writeStream);
        writeStream.Flush();
    }

    public Stream Read(string filename)
    {
        var fullPath = GetFullPath(filename);
        if (!Exists(fullPath))
        {
            throw new Exception("File does not exist");
        }

        return File.OpenRead(fullPath);
    }

    private string GetFullPath(string filename)
    {
        var directory = Path.GetFullPath(options.Value.DirectoryName, AppContext.BaseDirectory);
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, filename);
        return path;
    }
}
