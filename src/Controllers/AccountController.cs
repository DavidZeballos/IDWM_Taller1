using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using System.Security.Claims;

namespace IDWM_TallerAPI.Src.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // Método para obtener el ID del usuario autenticado desde los claims
        private int GetAuthenticatedUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Usuario no autenticado o ID inválido.");
            }

            return userId;
        }

        // Edita el perfil del usuario autenticado.
        // "editUser" son los nuevos datos del usuario.
        // Retorna un mensaje de éxito o error.
        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile([FromBody] EditUserDto editUser)
        {
            if (editUser == null)
            {
                return BadRequest("Los datos proporcionados no son válidos.");
            }

            try
            {
                var userId = GetAuthenticatedUserId();
                await _userService.EditUser(userId, editUser);
                return Ok("Perfil actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cambia la contraseña del usuario autenticado.
        // "changePasswordDto" son los datos de la contraseña nueva y de la actual.
        // Retorna un mensaje de éxito o error.
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto == null)
            {
                return BadRequest("Los datos proporcionados no son válidos.");
            }

            try
            {
                var userId = GetAuthenticatedUserId();
                await _userService.ChangeUserPassword(userId, changePasswordDto);
                return Ok("Contraseña actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
