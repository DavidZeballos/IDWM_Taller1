using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Models;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using IDWM_TallerAPI.Src.Interfaces.Service;


namespace IDWM_TallerAPI.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService service)
        {
            _productService = service;
        }

        // Retorna una lista de productos que coinciden con los parámetros de búsqueda.
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
            [FromQuery] string? name,
            [FromQuery] string? typeName,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
        try
        {
            var (products, totalItems) = await _productService.GetProducts(name, typeName, sortOrder, page, pageSize);
            
                Response.Headers["X-Total-Count"] = totalItems.ToString();
                Response.Headers["X-Current-Page"] = page.ToString();
                Response.Headers["X-Page-Size"] = pageSize.ToString();
                Response.Headers["X-Total-Pages"] = Math.Ceiling((double)totalItems / pageSize).ToString();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Agrega un nuevo producto.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(product.ProductTypeName))
                {
                    return BadRequest("El tipo de producto es obligatorio.");
                }

                product.ProductTypeName = product.ProductTypeName.ToLower(); // Normaliza el tipo de producto
                await _productService.AddProduct(product);
                return Ok("Producto agregado con éxito.");
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

        // Edita un producto existente.
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] EditProductDto editProduct)
        {
            try
            {
                await _productService.EditProduct(id, editProduct);
                return Ok("Producto actualizado con éxito.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        // Elimina un producto existente.
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok("Producto eliminado con éxito.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}