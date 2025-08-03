using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Client.Dtos;

namespace WebUI.Client.Services;

public interface IFileUploadService
{
    Task<string> UploadFile(IBrowserFile file);
}

public class FileUploadService : IFileUploadService
{
    private readonly HttpClient _http;

    const long MaxFileSize = 1024 * 1024 * 15;

    public FileUploadService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> UploadFile(IBrowserFile file)
    {
        var storedFileName = "";
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(file.OpenReadStream(MaxFileSize));
        content.Add(content: fileContent, name: "\"files\"", fileName: file.Name);

        var response = await _http.PostAsync("api/File", content);

        var newUploadResult = await response.Content.ReadFromJsonAsync<UploadResultDto>();

        if (newUploadResult != null)
        {
            storedFileName = newUploadResult.StoredFileName;
        }

        return storedFileName;
    }
}
