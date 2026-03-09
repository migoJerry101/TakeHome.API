using TakeHome.API.Models;

namespace TakeHome.API.Interface
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(string username, string password);
        Task<string> LoginAsync(string username, string password);
    }
}
