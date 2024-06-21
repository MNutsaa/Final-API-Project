using Forum.Entities;
using Microsoft.AspNetCore.Identity;

namespace Forum.Contracts
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllUsersAsync();
        Task<Users> GetUserByUsernNameAsync(string userName);
        Task<Users> GetUserByIdAsync(string userId);
        Task<bool> CheckPasswordAsync(Users user, string password);
        Task<IEnumerable<string>> GetRolesAsync(Users user);
        Task BanUserAsync(string userId);
        Task UnbanUserAsync(string userId);
        Task<IdentityResult> CreateAsync(Users user, string password);
        Task<bool> RoleExistsAsync(string role);
        Task AddToRoleAsync(Users user, string role);
        Task DeleteUserAsync(Users user);
        string GetAuthenticatedUserId();
        string GetAuthenticatedUserRole();
    }
}
