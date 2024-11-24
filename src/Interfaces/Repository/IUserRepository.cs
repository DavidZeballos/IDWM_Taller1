using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;


namespace IDWM_TallerAPI.Src.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserByEmail(string Email);
        Task<User?> GetUserById(int id);
        Task AddUser(User user);
        Task EditUser(User user);
        Task DeleteUser(User user);
    }
}