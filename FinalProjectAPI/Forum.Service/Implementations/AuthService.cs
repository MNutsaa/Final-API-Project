using AutoMapper;
using Forum.Contracts;
using Forum.Entities;
using Forum.Models.Identity;

namespace Forum.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IJwtGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private const string _adminRole = "Admin";
        private const string _customerRole = "Customer";

        public AuthService(IUserRepository userRepository, IJwtGenerator jwtTokenGenerator, MappingInitializer mappingInitializer)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _mapper = mappingInitializer.Initialize();
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto.UserName == null || loginRequestDto.Password == null)
            {
                throw new ArgumentNullException("You must indicate username and password");
            }

            var user = await _userRepository.GetUserByUsernNameAsync(loginRequestDto.UserName.ToLower());

            bool isValid = await _userRepository.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    Token = string.Empty,
                    User = null
                };
            }

            var roles = await _userRepository.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            var userDto = _mapper.Map<UserDto>(user);

            LoginResponseDto result = new()
            {
                User = userDto,
                Token = token
            };
            return result;
        }

        public async Task Register(RegistrationRequestDto registrationRequestDto)
        {
            Users users = _mapper.Map<Users>(registrationRequestDto);

            try
            {
                var result = await _userRepository.CreateAsync(users, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var userResult = await _userRepository.GetUserByUsernNameAsync(registrationRequestDto.UserName.ToLower());

                    if (userResult != null)
                    {
                        if (await _userRepository.RoleExistsAsync(_customerRole))
                        {
                            await _userRepository.AddToRoleAsync(userResult, _customerRole);
                        }

                        var userDto = _mapper.Map<UserGettingDto>(userResult);
                    }
                }
                else
                {
                    throw new Exception("Error occured");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RegisterAdmin(RegistrationRequestDto registrationRequestDto)
        {
            Users users = _mapper.Map<Users>(registrationRequestDto);

            try
            {
                var result = await _userRepository.CreateAsync(users, registrationRequestDto.Password);

                if (result.Succeeded)
                {
                    var userResult = await _userRepository.GetUserByUsernNameAsync(registrationRequestDto.UserName.ToLower());

                    if (userResult != null)
                    {
                        if (!await _userRepository.RoleExistsAsync(_adminRole))
                        {
                            await _userRepository.AddToRoleAsync(userResult, _adminRole);
                        }

                        var userDto = _mapper.Map<UserGettingDto>(userResult);
                    }
                }
                else
                {
                    throw new Exception("Error occured");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    }
