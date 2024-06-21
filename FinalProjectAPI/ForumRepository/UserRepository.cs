using Forum.Contracts;
using Forum.Entities;
using Forum.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ForumRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task BanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundExcpetion();
            }
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(100));
        }

        public async Task DeleteUserAsync(Users user)
        {
            if (user == null)
            {
                throw new UserNotFoundExcpetion();
            }
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
            {
                throw new UserNotFoundExcpetion();
            }

            return users;
        }

        public async Task<Users> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundExcpetion();
            }

            return user;
        }

        public async Task UnbanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundExcpetion();
            }
            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        public string GetAuthenticatedUserId()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                throw new UnauthorizedAccessException("Can't get credentials of unauthorzied user");
            }
        }
        public string GetAuthenticatedUserRole()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            else
            {
                throw new UnauthorizedAccessException("Can't get credentials of unauthorzied user");
            }
        }

        public async Task<Users> GetUserByUsernNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new UserNotFoundExcpetion();
            }
            return user;
        }

        public async Task<IdentityResult> CreateAsync(Users user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> RoleExistsAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task AddToRoleAsync(Users user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckPasswordAsync(Users user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(Users user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
