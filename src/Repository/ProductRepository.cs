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

    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Product>, int)> GetProducts(string? name, string? typeName, int? price, int page, int pageSize)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.ProductType);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(typeName))
            {
                query = query.Where(p => p.ProductType.Name.Equals(typeName));
            }
            if (price.HasValue)
            {
                query = query.Where(p => p.Price == price.Value);
            }

            int totalItems = await query.CountAsync();
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (products, totalItems);
        }

        public async Task<Product?> GetProductById(int id)
        {
            var product = await _context.Products.Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        
        public async Task EditProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
