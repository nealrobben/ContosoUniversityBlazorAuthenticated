namespace ContosoUniversityBlazor.Application.Common.Interfaces;

using ContosoUniversityBlazor.Application.Common.Models;
using global::System.Threading.Tasks;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
