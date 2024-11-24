using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Interfaces.Service
{
    public interface IProductService
    {
        public Task<(IEnumerable<ProductDto>, int)> GetProducts(string? name, string? typeName, int? price, int page, int pageSize);
        public Task<ProductDto> GetProductById(int id);
        public Task AddProduct(ProductDto product);
        public Task EditProduct(int id, EditProductDto editProduct);
        public Task DeleteProduct(int id);
    }
}