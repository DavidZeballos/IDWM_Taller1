using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using IDWM_TallerAPI.Src.Models;
using Microsoft.AspNetCore.Identity;

namespace IDWM_TallerAPI.Src.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapperService _mapperService;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, IMapperService mapperService, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _mapperService = mapperService;
            _userManager = userManager;
        }

        // Elimina un usuario
        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo eliminar el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // Modifica un usuario
        public async Task EditUser(int id, EditUserDto editUserDto)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            _mapperService.MapEditUserDtoToUser(editUserDto, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo editar el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // Cambia la contraseña del usuario
        public async Task ChangeUserPassword(int id, ChangePasswordDto changePasswordDto)
        {
            // Validar coincidencia de contraseñas
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                throw new InvalidOperationException("Las contraseñas no coinciden.");
            }

            // Obtener usuario
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            // Validar contraseña actual
            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
            if (!isCurrentPasswordValid)
            {
                throw new InvalidOperationException("La contraseña actual es incorrecta.");
            }

            // Cambiar contraseña
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo cambiar la contraseña: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // Obtiene todos los usuarios que se adecuen a un filtro de id, nombre, o género
        public async Task<IEnumerable<UserDto>> GetUsers(int? id = null, string? name = null, string? gender = null)
        {
            var users = await _userRepository.GetUsers();

            // Filtrar usuarios opcionalmente según parámetros
            if (id.HasValue)
                users = users.Where(u => u.Id == id.Value);
            if (!string.IsNullOrWhiteSpace(name))
                users = users.Where(u => u.UserName?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false);
            if (!string.IsNullOrWhiteSpace(gender))
                users = users.Where(u => u.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));

            return _mapperService.UsersToUserDto(users);
        }

        // Cambia el estado de un usuario
        public async Task ToggleUserStatus(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                throw new InvalidOperationException("No se puede cambiar el estado de un administrador.");
            }

            user.Status = !user.Status;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"No se pudo actualizar el estado del usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
