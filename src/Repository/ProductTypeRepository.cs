using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Data;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using IDWM_TallerAPI.Src.Models;
using CloudinaryDotNet.Actions;

namespace IDWM_TallerAPI.Src.Repository
{

    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly DataContext _context;

        public ProductTypeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ProductType?> GetProductTypeById(int id)
        {
            return await _context.ProductTypes.FindAsync(id);
        }

        public async Task<ProductType?> GetProductTypeByName(string name)
        {
            return await _context.ProductTypes
                .FirstOrDefaultAsync(pt => pt.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<ProductType>> GetAllProductTypes()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}
