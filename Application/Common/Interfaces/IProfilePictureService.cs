namespace Application.Common.Interfaces;

using System.IO;
using System.Threading.Tasks;

public interface IProfilePictureService
{
    Task WriteImageFile(string name, MemoryStream ms);
    Task<byte[]> GetImageFile(string fullName);
    void DeleteImageFile(string name);
}
