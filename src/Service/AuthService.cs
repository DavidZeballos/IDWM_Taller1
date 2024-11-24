
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using IDWM_TallerAPI.Src.Interfaces.Service;

namespace IDWM_TallerAPI.Src.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;

        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IMapperService _mapperService;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IConfiguration configuration, ITokenService tokenService, IUserRepository userRepository, IMapperService mapperService, IRoleRepository roleRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _configuration = configuration;
            _mapperService = mapperService;
            _roleRepository = roleRepository;
        }
        public async Task<string> RegisterUser(RegisterUserDto registerUserDto)
        {
            var mappedUser = _mapperService.RegisterUserDtoToUser(registerUserDto);
            if(_userRepository.VerifyUserByEMail(mappedUser.Email).Result){
                throw new Exception("El email ingresado ya existe.");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password, salt);
            mappedUser.Password = passwordHash;

            var role = await _roleRepository.GetRoleByName("User");
            if(role == null){
                throw new Exception("Error del servidor, intentelo más tarde.");
            }
            mappedUser.RoleId = role.Id;
            
            await _userRepository.AddUser(mappedUser);
            var user = await _userRepository.GetUserByEmail(mappedUser.Email);
            if(user == null){
                return "Error del servidor, intentelo más tarde.";
            }
            
            var token = _tokenService.CreateToken(user);
            return token;
            
        }

        public async Task<string> LoginUser (LoginDto loginDto){
            var user = await _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null) {
                throw new Exception("El usuario no existe.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password)){
                throw new Exception("La contraseña es incorrecta.");
            }

            var token = _tokenService.CreateToken(user);
            return token;
            
        }
    }
}