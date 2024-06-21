using AutoMapper;
using Forum.Contracts;
using Forum.Models.Identity;

namespace Forum.Service.Implementations
{
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, MappingInitializer mappingInitializer)
        {
            _mapper = mappingInitializer.Initialize();
            _userRepository = userRepository;
        }
        public async Task<bool> BanUserAsync(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentException("User not found.");
            }
            var userRole = _userRepository.GetAuthenticatedUserRole();

            if (userRole != "Admin".Trim())
            {
                throw new UnauthorizedAccessException("User can't ban other user.");
            }

            try
            {
                await _userRepository.BanUserAsync(userId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task DeleteUserAsync(string userId)
        {

            if (userId is null)
            {
                throw new ArgumentNullException("Invalid argument passed");
            }
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var authenticatedId = _userRepository.GetAuthenticatedUserId();

            if (authenticatedId is null)
            {
                throw new UnauthorizedAccessException("Must be logged in to delete account");
            }
            if (user.Id != authenticatedId)
            {
                throw new ArgumentException("You can't delete other account");
            }
            else
            {
                await _userRepository.DeleteUserAsync(user);
            }
        }

        public async Task<List<UserGettingDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            var mappedUsers = _mapper.Map<List<UserGettingDto>>(users);
            return mappedUsers;
        }
        public async Task<UserGettingDto> GetUserByIdAsync(string userId)
        {
            var raw = await _userRepository.GetUserByIdAsync(userId);

            if (raw == null)
            {
                throw new ArgumentException("User not found");
            }
            var user = _mapper.Map<UserGettingDto>(raw);

            return user;
        }

        public async Task<bool> UnbanUserAsync(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentException("Invalid argument passed");
            }
            var userRole = _userRepository.GetAuthenticatedUserRole();

            if (userRole != "Admin".Trim())
            {
                throw new Exception("You can't unban user.");
            }

            try
            {
                await _userRepository.UnbanUserAsync(userId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
