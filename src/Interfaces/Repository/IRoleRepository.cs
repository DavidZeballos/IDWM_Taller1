using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Interfaces.Repository
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleById(int id);
        Task<Role?> GetRoleByName(string name);
    }
}