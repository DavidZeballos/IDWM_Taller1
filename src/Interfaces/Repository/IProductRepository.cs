using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;


namespace IDWM_TallerAPI.Src.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product>, int)> GetProducts(string? name, string? typeName, string? sortOrder, int page, int pageSize);
        Task<Product?> GetProductById(int id);

        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task EditProduct(Product product);
        Task DeleteProduct(Product product);
    }
}