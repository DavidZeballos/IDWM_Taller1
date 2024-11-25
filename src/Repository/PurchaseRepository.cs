using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Data;
using IDWM_TallerAPI.Src.Models;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace IDWM_TallerAPI.Src.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DataContext _context;
        
        public PurchaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddPurchase(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task<Purchase?> GetPurchaseById(int id)
        {
            return await _context.Purchases
                .Include(p => p.Product.ProductType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesById(int userId)
        {
            return await _context.Purchases
                .Include(p => p.Product.ProductType)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchases()
        {
            return await _context.Purchases
                .Include(p => p.Product.ProductType)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesByQuery(int? id, DateTime? date, string? name)
        {
            var query = _context.Purchases
                .Include(p => p.Product)
                .Include(p => p.User)
                .AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(p => p.Id == id.Value);
            }

            if (date.HasValue)
            {
                query = query.Where(p => p.Date.Date == date.Value.Date);
            }

            if (name != null)
            {
                query = query.Where(p => p.User.UserName == name);
            }

            return await query.ToListAsync();
        }
    }
}
