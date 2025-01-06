using System.Collections.Generic;
using System.Linq;
using IDWM_TallerAPI.Src.Models;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using IDWM_TallerAPI.Src.Interfaces.Repository;

namespace IDWM_TallerAPI.Src.Service
{
    public class MapperService : IMapperService
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public MapperService(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        // Convierte ProductDto a Product y valida el tipo de producto.
        public Product ProductDtoToProduct(ProductDto productDto)
        {
            var productType = _productTypeRepository.GetProductTypeByName(productDto.ProductTypeName)?.Result;
            if (productType == null)
            {
                throw new InvalidOperationException($"El tipo de producto '{productDto.ProductTypeName}' no es válido.");
            }

            return new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                InStock = productDto.InStock,
                ImageURL = productDto.ImageURL,
                ProductTypeId = productType.Id,
                ProductType = productType
            };
        }

        // Convierte Product a ProductDto.
        public ProductDto ProductToProductDto(Product product)
        {
            return new ProductDto
            {
                Name = product.Name,
                Price = product.Price,
                InStock = product.InStock,
                ImageURL = product.ImageURL,
                ProductTypeName = product.ProductType.Name
            };
        }

        public IEnumerable<ProductDto> ProductsToProductDto(IEnumerable<Product> products)
        {
            return products.Select(ProductToProductDto);
        }

        // Convierte un Purchase a VoucherDto.
        public VoucherDto PurchaseToVoucherDto(Purchase purchase)
        {
            if (purchase == null)
            {
                throw new Exception("Compra no encontrada.");
            }

            return new VoucherDto
            {
                PurchaseId = purchase.Id,
                PurchaseDate = purchase.Date,
                TotalPrice = purchase.TotalPrice,
                Products = new List<VoucherProductDto>
                {
                    new VoucherProductDto
                    {
                        ProductId = purchase.Product.Id,
                        ProductName = purchase.Product.Name,
                        ProductType = purchase.Product.ProductType.Name,
                        ProductPrice = purchase.Product.Price,
                        Quantity = purchase.Quantity
                    }
                }
            };
        }

        public IEnumerable<VoucherDto> PurchasesToVoucherDto(IEnumerable<Purchase> purchases)
        {
            return purchases.Select(PurchaseToVoucherDto);
        }

        // Convierte RegisterUserDto a User.
        public User RegisterUserDtoToUser(RegisterUserDto registerUserDto)
        {
            return new User
            {
                UserName = registerUserDto.UserName,
                Rut = registerUserDto.Rut,
                Email = registerUserDto.Email,
                DateOfBirth = registerUserDto.DateOfBirth,
                Gender = registerUserDto.Gender,
                Status = true
            };
        }

        public UserDto UserToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Rut = user.Rut,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Status = user.Status
            };
        }

        public IEnumerable<UserDto> UsersToUserDto(IEnumerable<User> users)
        {
            return users.Select(UserToUserDto);
        }

        public void MapEditProductDtoToProduct(EditProductDto editProductDto, Product product)
        {
            if (!string.IsNullOrWhiteSpace(editProductDto.Name))
                product.Name = editProductDto.Name;

            if (editProductDto.Price.HasValue)
                product.Price = editProductDto.Price.Value;

            if (editProductDto.InStock.HasValue)
                product.InStock = editProductDto.InStock.Value;

            if (!string.IsNullOrWhiteSpace(editProductDto.ImageURL))
                product.ImageURL = editProductDto.ImageURL;

            if (!string.IsNullOrWhiteSpace(editProductDto.ProductTypeName))
            {
                var productType = _productTypeRepository.GetProductTypeByName(editProductDto.ProductTypeName)?.Result;
                if (productType == null)
                {
                    throw new InvalidOperationException($"El tipo de producto '{editProductDto.ProductTypeName}' no es válido.");
                }

                product.ProductTypeId = productType.Id;
                product.ProductType = productType;
            }
        }

        public void MapEditUserDtoToUser(EditUserDto editUserDto, User user)
        {
            if (!string.IsNullOrWhiteSpace(editUserDto.Name))
                user.UserName = editUserDto.Name;

            if (!string.IsNullOrWhiteSpace(editUserDto.Gender))
                user.Gender = editUserDto.Gender;

            if (editUserDto.DateOfBirth.HasValue)
                user.DateOfBirth = editUserDto.DateOfBirth.Value;
        }
    }
}
