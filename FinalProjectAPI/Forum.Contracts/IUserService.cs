using Forum.Models.Identity;

namespace Forum.Contracts
{
    public interface IUserService
    {
        Task<List<UserGettingDto>> GetAllUsersAsync();
        Task<UserGettingDto> GetUserByIdAsync(string userId);
        Task<bool> BanUserAsync(string userId);
        Task<bool> UnbanUserAsync(string userId);
        Task DeleteUserAsync(string userId);
    }
}
