using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapperService _mapperService;

        public ProductService(IProductRepository productRepository, IMapperService mapperService)
        {
            _productRepository = productRepository;
            _mapperService = mapperService;
        }

        public async Task<(IEnumerable<ProductDto>, int)> GetProducts(string? name, string? typeName, int? price, int page, int pageSize)
        {
            var (products, totalItems) = await _productRepository.GetProducts(name, typeName, price, page, pageSize);
            var mappedProducts = _mapperService.ProductsToProductDto(products.Where(p => p != null)!);
            return (mappedProducts, totalItems);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"El producto con ID {id} no fue encontrado.");
            }

            return _mapperService.ProductToProductDto(product);
        }

        public async Task AddProduct(ProductDto productDto)
        {
            if (productDto == null) throw new ArgumentNullException(nameof(productDto));

            // Validar si ya existe un producto con el mismo nombre
            var existingProduct = await _productRepository.GetProducts(productDto.Name, null, null, 1, 1);
            if (existingProduct.Item1.Any())
            {
                throw new InvalidOperationException("Ya existe un producto con el mismo nombre.");
            }

            var product = _mapperService.ProductDtoToProduct(productDto);
            await _productRepository.AddProduct(product);
        }

        public async Task EditProduct(int id, EditProductDto editProductDto)
        {
            if (editProductDto == null) throw new ArgumentNullException(nameof(editProductDto));

            var existingProduct = await _productRepository.GetProductById(id)
                ?? throw new KeyNotFoundException($"El producto con ID {id} no fue encontrado.");

            // Mapear cambios del DTO al modelo existente
            _mapperService.MapEditProductDtoToProduct(editProductDto, existingProduct);

            await _productRepository.EditProduct(existingProduct);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                throw new KeyNotFoundException("El producto no existe.");
            }

            await _productRepository.DeleteProduct(product);
        }
    }
}