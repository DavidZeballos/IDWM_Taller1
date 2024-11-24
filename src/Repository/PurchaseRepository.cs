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
            var purchase = await _context.Purchases.FindAsync(id);
            return purchase;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesById(int id)
        {
            var purchases = await _context.Purchases.Include(p => p.Product.ProductType)
                .Include(p => p.User.Role)
                .Where(p=> p.UserId == id)
                .OrderByDescending(p=>p.Date)
                .ToListAsync();;
            
            return purchases;
        }
        public async Task<IEnumerable<Purchase>> GetAllPurchases()
        {
            var purchases = await _context.Purchases.Include(p => p.Product.ProductType)
                .Include(p => p.User.Role)
                .ToListAsync();

            return purchases;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesByQuery(int? id, DateTime? date, int? price)
        {
            IQueryable<Purchase> query = _context.Purchases.Include(p => p.Product).Include(p=> p.User);

            if (id.HasValue)
            {
                query = query.Where(p => p.Id == id.Value);
            }
            if (date.HasValue)
            {
                query = query.Where(p => p.Date.Date == date.Value.Date);
            }
            if (price.HasValue)
            {
                query = query.Where(p => p.TotalPrice == price.Value);
            }

            var purchases = await query.ToListAsync();
            return purchases;
        }
    }
}