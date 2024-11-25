using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Models;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using IDWM_TallerAPI.Src.Interfaces.Service;

namespace IDWM_TallerAPI.Src.Service
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IMapperService _mapperService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository, IMapperService mapperService, IProductRepository productRepository, IUserRepository userRepository){
            _purchaseRepository = purchaseRepository;
            _mapperService = mapperService;
            _productRepository = productRepository;
            _userRepository = userRepository;
            
        }

        public async Task<VoucherDto> AddPurchase(int productId, int userId, MakePurchaseDto makePurchaseDto)
        {
            // Validar el producto
            var product = await _productRepository.GetProductById(productId)
                ?? throw new KeyNotFoundException("El producto no fue encontrado.");

            // Validar el usuario
            var user = await _userRepository.GetUserById(userId)
                ?? throw new KeyNotFoundException("El usuario no fue encontrado.");

            // Validar stock
            if (product.InStock < makePurchaseDto.Quantity)
            {
                throw new InvalidOperationException("Stock insuficiente para completar la compra.");
            }

            // Crear compra
            var totalPrice = product.Price * makePurchaseDto.Quantity;
            var purchase = new Purchase
            {
                Date = DateTime.Now,
                ProductId = productId,
                Quantity = makePurchaseDto.Quantity,
                TotalPrice = totalPrice,
                UserId = userId
            };

            // Actualizar stock del producto
            product.InStock -= makePurchaseDto.Quantity;

            // Guardar cambios
            await _purchaseRepository.AddPurchase(purchase);
            await _productRepository.UpdateProduct(product);

            // Mapear la compra a DTO y retornar
            return _mapperService.PurchaseToVoucherDto(purchase);
        }

        public async Task<IEnumerable<VoucherDto>> GetAllPurchases()
        {
            var purchases = await _purchaseRepository.GetAllPurchases();
            return _mapperService.PurchasesToVoucherDto(purchases);
        }

        public async Task<IEnumerable<VoucherDto>> GetPurchasesById(int id)
        {
            var purchases = await _purchaseRepository.GetPurchasesById(id);
            return _mapperService.PurchasesToVoucherDto(purchases);
        }

        public async Task<IEnumerable<VoucherDto>> GetPurchasesByQuery(int? id, DateTime? date, string? name)
        {
            var purchases = await _purchaseRepository.GetPurchasesByQuery(id, date, name);
            return _mapperService.PurchasesToVoucherDto(purchases);
        }

    }
}