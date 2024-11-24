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
        // Retorna un Token JWT si el registro es exitoso.
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authService.RegisterUser(registerUserDto);
                return Ok(token);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // Error específico
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Error genérico
            }
        }

        // Inicia sesión para un usuario existente.
        // Retorna un Token JWT si las credenciales son correctas.
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authService.LoginUser(loginDto);
                return Ok(token);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message); // Error de autenticación
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Error genérico
            }
        }
    }
}