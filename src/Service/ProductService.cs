using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using IDWM_TallerAPI.Src.Models;

namespace IDWM_TallerAPI.Src.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMapperService _mapperService;

        public ProductService(IProductRepository productRepository, IProductTypeRepository productTypeRepository, IMapperService mapperService)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _mapperService = mapperService;
        }

        // Obtiene todos los productos, filtrando por nombre, tipo, y orden ascendente o descendente (según precio)
        public async Task<(IEnumerable<ProductDto>, int)> GetProducts(string? name, string? typeName, string? sortOrder, int page, int pageSize)
        {
            var (products, totalItems) = await _productRepository.GetProducts(name, typeName, sortOrder, page, pageSize);
            var mappedProducts = _mapperService.ProductsToProductDto(products.Where(p => p != null)!);
            return (mappedProducts, totalItems);
        }

        // Obtiene un producto por su id
        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"El producto con ID {id} no fue encontrado.");
            }

            return _mapperService.ProductToProductDto(product);
        }

        // Añade un nuevo producto
        public async Task AddProduct(ProductDto productDto)
        {
            if (productDto == null) throw new ArgumentNullException(nameof(productDto));

            // Validar si el ProductTypeId es válido
            var productType = await _productTypeRepository.GetProductTypeByName(productDto.ProductTypeName);
            if (productType == null)
            {
                throw new InvalidOperationException("El tipo de producto especificado no existe.");
            }

            // Validar si ya existe un producto con el mismo nombre
            var existingProduct = await _productRepository.GetProducts(productDto.Name, null, null, 1, 1);
            if (existingProduct.Item1.Any())
            {
                throw new InvalidOperationException("Ya existe un producto con el mismo nombre.");
            }

            var product = _mapperService.ProductDtoToProduct(productDto);
            await _productRepository.AddProduct(product);
        }

        // Modifica un producto
        public async Task EditProduct(int id, EditProductDto editProductDto)
        {
            if (editProductDto == null) throw new ArgumentNullException(nameof(editProductDto));

            // Obtener el producto existente
            var existingProduct = await _productRepository.GetProductById(id)
                ?? throw new KeyNotFoundException($"El producto con ID {id} no fue encontrado.");

            // Validar si el ProductTypeName es válido (si se proporcionó)
            if (!string.IsNullOrWhiteSpace(editProductDto.ProductTypeName))
            {
                var productType = await _productTypeRepository.GetProductTypeByName(editProductDto.ProductTypeName);
                if (productType == null)
                {
                    throw new InvalidOperationException($"El tipo de producto '{editProductDto.ProductTypeName}' no existe.");
                }

                // Asignar el ID del tipo de producto al modelo existente
                existingProduct.ProductTypeId = productType.Id;
            }

            // Validar si ya existe otro producto con el mismo nombre y tipo
            if (!string.IsNullOrWhiteSpace(editProductDto.Name) && !string.IsNullOrWhiteSpace(editProductDto.ProductTypeName))
            {
                var duplicateProduct = await _productRepository.GetProducts(
                    editProductDto.Name, editProductDto.ProductTypeName, null, 1, 1);

                if (duplicateProduct.Item1.Any(p => p.Id != id))
                {
                    throw new InvalidOperationException(
                        "Ya existe otro producto con el mismo nombre y tipo.");
                }
            }

            // Mapear cambios del DTO al modelo existente
            _mapperService.MapEditProductDtoToProduct(editProductDto, existingProduct);

            // Guardar los cambios
            await _productRepository.EditProduct(existingProduct);
        }

        // Elimina un producto por su id
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