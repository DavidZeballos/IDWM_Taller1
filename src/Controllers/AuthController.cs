using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDWM_TallerAPI.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Registra un nuevo usuario.
        // Retorna un Token JWT si el registro es exitoso.¿
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserDto registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { code = 400, message = "Datos de registro inválidos." });
            }

            try
            {
                var token = await _authService.RegisterUser(registerUser);
                return Ok(new { token });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { code = 409, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = "Error interno del servidor.", details = ex.Message });
            }
        }

        // Inicia sesión para un usuario existente.
        // Retorna un Token JWT si las credenciales son correctas.
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Datos de inicio de sesión inválidos.",
                    details = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var token = await _authService.LoginUser(login);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new
                    {
                        code = 401,
                        message = "Error: No se generó un token válido."
                    });
                }

                return Ok(new { token });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new
                {
                    code = 401,
                    message = "Credenciales inválidas.",
                    details = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Usuario no encontrado.",
                    details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = "Error interno del servidor durante el inicio de sesión.",
                    details = ex.Message
                });
            }
        }
    }
}