using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using IDWM_TallerAPI.Src.Interfaces.Repository;

namespace IDWM_TallerAPI.Src.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapperService _mapperService;

        public UserService(IUserRepository userRepository, IMapperService mapperService)
        {
            _userRepository = userRepository;
            _mapperService = mapperService;
        }

        public async Task<IEnumerable<UserDto>> GetUsers(int? id , string? name , string? gender)
        {
            var users = await _userRepository.GetUsers();
            var mappedUsers = _mapperService.UsersToUserDto(users);
            return mappedUsers;
        }

        public async Task EditUser(int id, EditUserDto editUserDto)
        {
            // Validar género si se proporciona
            if (!string.IsNullOrWhiteSpace(editUserDto.Gender) &&
                !new[] { "Femenino", "Masculino", "Prefiero no decirlo", "Otro" }.Contains(editUserDto.Gender))
            {
                throw new InvalidOperationException("El género debe ser uno de los siguientes: Femenino, Masculino, Prefiero no decirlo u Otro.");
            }

            var existingUser = await _userRepository.GetUserById(id)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");
            
            _mapperService.MapEditUserDtoToUser(editUserDto, existingUser);
            await _userRepository.EditUser(existingUser);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            await _userRepository.DeleteUser(user);
        }

        public async Task ChangeUserStatus(int id)
        {
            var user = await _userRepository.GetUserById(id)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            if (user.RoleId == 1)
            {
                throw new InvalidOperationException("No se puede cambiar el estado de un administrador.");
            }

            user.Status = !user.Status;
            await _userRepository.EditUser(user);
        }

        public async Task ChangeUserPassword(int id, ChangePasswordDto changePasswordDto)
        {
            // Validar coincidencia de contraseñas
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                throw new InvalidOperationException("Las contraseñas no coinciden.");
            }

            // Obtener usuario
            var user = await _userRepository.GetUserById(id)
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            // Validar contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.Password))
            {
                throw new InvalidOperationException("La contraseña actual es incorrecta.");
            }

            // Actualizar contraseña
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            // Guardar cambios
            await _userRepository.EditUser(user);
        }
    }
}