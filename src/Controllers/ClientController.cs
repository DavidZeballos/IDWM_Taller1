using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Repository;
using IDWM_TallerAPI.Src.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet.Actions;

namespace IDWM_TallerAPI.Src.Controllers
{
    [ApiController]
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IUserService _userService;
        private const int PageSize = 10; 

        public ClientController(IUserService userService)
        {
            _userService = userService;
        }


        /// Obtiene una lista de clientes.
        /// "id" es la ID opcional del cliente.
        /// "name" es el Nombre opcional del cliente.
        /// "gender" es el Género opcional del cliente.
        /// Retorna una lista de clientes que coinciden con los parámetros de búsqueda.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetClients([FromQuery] int? id, [FromQuery] string? name, [FromQuery] string? gender)
        {
            try{ 
                var clients = await _userService.GetUsers(id, name, gender);
                return Ok(clients);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        /// Cambia el estado de un cliente.
        /// "id" es la ID del cliente cuyo estado se va a cambiar.
        /// Retorna un mensaje de éxito o error.
        [HttpPut("ChangeStatus/{id}")]
        public async Task<ActionResult<string>> ClientStatus(int id)
        {
            try{ 
                var clients = await _userService.ChangeUserStatus(id);
                return Ok(clients);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


    }
}
