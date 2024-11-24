using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;

namespace IDWM_TallerAPI.Src.Interfaces.Service
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetUsers(int? id, string? name, string? gender);
        public Task EditUser(int id, EditUserDto editUser);
        public Task DeleteUser(int id);
        public Task ToggleUserStatus(int id);
        public Task ChangeUserPassword(int id, ChangePasswordDto changePasswordDto);
    }
}