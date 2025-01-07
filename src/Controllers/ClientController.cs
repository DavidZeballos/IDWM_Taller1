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
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IUserService _userService;
        private const int PageSize = 10; 

        public ClientController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst("Id")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("No se encontró el claim 'Id' en el token.");
                }

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return BadRequest("El claim 'Id' no es válido.");
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el perfil: {ex.Message}");
            }
        }

        /// Obtiene una lista de clientes.
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetClients(
            [FromQuery] int? id, 
            [FromQuery] string? name, 
            [FromQuery] string? gender, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetUsers(id, name, gender);
                var paginatedUsers = users.Skip((page - 1) * pageSize).Take(pageSize);
                Response.Headers["X-Total-Count"] = users.Count().ToString();
                return Ok(paginatedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cambiar el estado de un cliente (habilitar/deshabilitar)
        [HttpPut("{id}/status")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ToggleClientStatus(int id)
        {
            await _userService.ToggleUserStatus(id);
            return Ok($"Estado del cliente con ID {id} actualizado.");
        }

        // Eliminar un cliente
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            await _userService.DeleteUser(id);
            return Ok($"Cliente con ID {id} eliminado.");
        }
    }
}
