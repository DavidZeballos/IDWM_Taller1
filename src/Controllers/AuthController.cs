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

        /// Registra un nuevo usuario.
        /// "registerUserDto" son los datos del usuario a registrar.
        /// Retorna un Token JWT si el registro es exitoso.
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterUserDto registerUserDto){
            try{
                var response = await _authService.RegisterUser(registerUserDto);
                return Ok(response);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        /// Inicia sesión para un usuario existente.
        /// "loginDto" son las credenciales de inicio de sesión.
        /// Retorna un Token JWT si las credenciales son correctas.
        [HttpPost("login")]
        public async Task<IResult> Login (LoginDto loginDto){
            try{
                var response = await _authService.LoginUser(loginDto);
                return TypedResults.Ok(response);
            }
            catch(Exception ex){
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}