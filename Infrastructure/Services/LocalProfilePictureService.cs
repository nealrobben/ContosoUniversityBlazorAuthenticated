using Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class LocalProfilePictureService : IProfilePictureService
{
    private readonly string _path;

    public LocalProfilePictureService(IWebHostEnvironment env)
    {
        _path = Path.Combine(env.ContentRootPath, "Img", "ProfilePictures");
    }

    public async Task WriteImageFile(string name, MemoryStream ms)
    {
        var fullName = Path.Combine(_path, name);

        ms.Position = 0;
        await using FileStream fs = new(fullName, FileMode.Create);
        await ms.CopyToAsync(fs);
    }

    public async Task<byte[]> GetImageFile(string fullName)
    {
        return await File.ReadAllBytesAsync(fullName);
    }

    public Task DeleteImageFile(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Task.CompletedTask;

        var fullName = Path.Combine(_path, name);

        if (File.Exists(fullName))
            File.Delete(fullName);

        return Task.CompletedTask;
    }
}
