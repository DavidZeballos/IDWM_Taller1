using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using IDWM_TallerAPI.Src.Models;
using Microsoft.AspNetCore.Identity;

namespace IDWM_TallerAPI.Src.Service
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AuthService(ITokenService tokenService, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (registerUserDto == null)
            {
                throw new ArgumentNullException(nameof(registerUserDto), "Los datos de registro no pueden estar vacíos.");
            }

            // Validar si el usuario ya existe
            if (await _userManager.FindByEmailAsync(registerUserDto.Email) != null)
            {
                throw new InvalidOperationException("El correo electrónico ya está registrado.");
            }

            // Crear usuario
            var user = new User
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                Rut = registerUserDto.Rut,
                DateOfBirth = registerUserDto.DateOfBirth,
                Gender = registerUserDto.Gender,
                Status = true
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error al crear el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Verificar si el rol existe
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                throw new InvalidOperationException("El rol 'User' no está configurado en el sistema.");
            }

            // Asignar rol
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException($"Error al asignar el rol 'User': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }

            // Generar token
            var roles = await _userManager.GetRolesAsync(user);
            return _tokenService.CreateToken(user, roles);
        }

        public async Task<string> LoginUser(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto), "Los datos de inicio de sesión no pueden estar vacíos.");
            }

            // Buscar usuario por email
            var user = await _userManager.FindByEmailAsync(loginDto.Email)
                ?? throw new KeyNotFoundException("El usuario no existe.");

            // Validar contraseña
            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidPassword)
            {
                throw new InvalidOperationException("La contraseña es incorrecta.");
            }

            // Generar y retornar el token
            var roles = await _userManager.GetRolesAsync(user);
            return _tokenService.CreateToken(user, roles);
        }
    }
}
