using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Repository;
using IDWM_TallerAPI.Src.Interfaces.Service;
using IDWM_TallerAPI.Src.Models;
using Microsoft.AspNetCore.Authorization;

namespace IDWM_TallerAPI.Src.Controllers
{
    [ApiController]
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public SaleController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        /// Obtiene una lista de compras basadas en los parámetros de búsqueda.
        /// "id" es la ID opcional de la compra.
        /// "date" es la fecha opcional de la compra.
        /// "price" es el precio opcional de la compra.
        /// Retorna una lista de compras que coinciden con los parámetros de búsqueda.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchasesByQuery([FromQuery] int? id, [FromQuery] DateTime? date, [FromQuery] int? price)
        {
            try{ 
                var purchases = await _purchaseService.GetPurchasesByQuery(id, date, price); 
                return Ok(purchases);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
