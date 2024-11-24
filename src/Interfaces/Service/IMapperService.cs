using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;
using IDWM_TallerAPI.Src.DTOs;


namespace IDWM_TallerAPI.Src.Interfaces.Service
{
    public interface IMapperService
    {
        public User RegisterUserDtoToUser(RegisterUserDto registerUserDto);
        public UserDto UserToUserDto(User user);
        public IEnumerable<UserDto> UsersToUserDto(IEnumerable<User> users);
        public VoucherDto PurchaseToVoucherDto(Purchase purchase);
        public IEnumerable<VoucherDto> PurchasesToVoucherDto(IEnumerable<Purchase> purchase);
        public Product ProductDtoToProduct(ProductDto productDto);
        public ProductDto ProductToProductDto(Product product);
        public IEnumerable<ProductDto> ProductsToProductDto(IEnumerable<Product> products); 

        public void MapEditProductDtoToProduct(EditProductDto editProductDto, Product product);
        public void MapEditUserDtoToUser(EditUserDto editUserDto, User user);
    }
}