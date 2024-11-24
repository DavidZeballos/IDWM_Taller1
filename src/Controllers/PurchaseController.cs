using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Helpers;
using IDWM_TallerAPI.Src.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;

namespace IDWM_TallerAPI.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IProductService _productService;

        public PurchaseController(IPurchaseService purchaseService, IProductService productService)
        {
            _purchaseService = purchaseService;
            _productService = productService;
        }

        // Visualiza el carrito de compra.
        // Muestra una lista de productos añadidos al carrito junto con el total a pagar.
        [HttpGet("cart")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<CartItemDto>> GetCart()
        {
            var cart = CookieHelper.GetCookie<List<CartItemDto>>(Request, "cart") ?? new List<CartItemDto>();
            return Ok(cart);
        }

        // Añade un producto al carrito de compra.
        [HttpPost("cart/add")]
        [AllowAnonymous]
        public async Task<ActionResult> AddToCart([FromBody] CartItemDto cartItem)
        {
            // Verifica si el producto existe y está disponible en stock
            var product = await _productService.GetProductById(cartItem.ProductId);
            if (product == null || product.InStock < cartItem.Quantity)
            {
                return BadRequest("El producto no existe o no tiene suficiente stock.");
            }

            var cart = CookieHelper.GetCookie<List<CartItemDto>>(Request, "cart") ?? new List<CartItemDto>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == cartItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                cart.Add(cartItem);
            }

            CookieHelper.SetCookie(Response, "cart", cart);
            return Ok("Producto añadido al carrito.");
        }

        // Actualiza la cantidad de un producto en el carrito.
        [HttpPut("cart/update")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateCart([FromBody] CartItemDto cartItem)
        {
            var cart = CookieHelper.GetCookie<List<CartItemDto>>(Request, "cart") ?? new List<CartItemDto>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == cartItem.ProductId);
            if (existingItem == null)
            {
                return NotFound("El producto no está en el carrito.");
            }

            // Verifica si hay suficiente stock
            var product = await _productService.GetProductById(cartItem.ProductId);
            if (product == null || product.InStock < cartItem.Quantity)
            {
                return BadRequest("No hay suficiente stock para la cantidad solicitada.");
            }

            existingItem.Quantity = cartItem.Quantity;
            if (existingItem.Quantity <= 0)
            {
                cart.Remove(existingItem);
            }

            CookieHelper.SetCookie(Response, "cart", cart);
            return Ok("Cantidad actualizada.");
        }

        // Elimina un producto del carrito.
        [HttpDelete("cart/remove/{productId}")]
        [AllowAnonymous]
        public ActionResult RemoveFromCart(int productId)
        {
            var cart = CookieHelper.GetCookie<List<CartItemDto>>(Request, "cart") ?? new List<CartItemDto>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem == null)
            {
                return NotFound("El producto no está en el carrito.");
            }

            cart.Remove(existingItem);
            CookieHelper.SetCookie(Response, "cart", cart);
            return Ok("Producto eliminado del carrito.");
        }

        // Realiza el pago de todos los productos en el carrito.
        // Vacía el carrito después de procesar la compra.
        [HttpPost("cart/checkout")]
        [Authorize]
        public async Task<ActionResult> Checkout()
        {
            var cart = CookieHelper.GetCookie<List<CartItemDto>>(Request, "cart") ?? new List<CartItemDto>();
            if (!cart.Any())
            {
                return BadRequest("El carrito está vacío.");
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == "Id").Value);

            try
            {
                foreach (var item in cart)
                {
                    var purchaseDto = new MakePurchaseDto { Quantity = item.Quantity };
                    await _purchaseService.AddPurchase(item.ProductId, userId, purchaseDto);
                }

                // Vaciar el carrito
                CookieHelper.SetCookie(Response, "cart", new List<CartItemDto>());
                return Ok("Compra realizada con éxito.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// Obtiene una lista de vouchers de compra del usuario autenticado.
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VoucherDto>>> GetPurchases()
        {
            try{ 
                var userId = int.Parse(User.Claims.First(c=>c.Type == "Id").Value);
                var purchases = await _purchaseService.GetPurchasesById(userId); 
                return Ok(purchases);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        // Obtiene todas las compras realizadas (admin).
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetAllPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchases();
            return Ok(purchases);
        }
    }
}
