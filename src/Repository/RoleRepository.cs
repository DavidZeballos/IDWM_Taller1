using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;
using IDWM_TallerAPI.Src.Data;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace IDWM_TallerAPI.Src.Repository
{
    public class RoleRepository : IRoleRepository
    {

        private readonly DataContext _context;

        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleById(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return role;
        }

        public async Task<Role?> GetRoleByName(string name)
        {
            var role = await _context.Roles.Where(r => r.Name == name).FirstOrDefaultAsync();
            return role;
        }
    }
}