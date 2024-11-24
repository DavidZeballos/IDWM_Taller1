using System.Collections.Generic;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;
using Microsoft.AspNetCore.Identity;


namespace IDWM_TallerAPI.Src.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
        Task<IdentityResult> AddUser(User user);
        Task<IdentityResult> EditUser(User user);
        Task<IdentityResult> DeleteUser(User user);
    }
}