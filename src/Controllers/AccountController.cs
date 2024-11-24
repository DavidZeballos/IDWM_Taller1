using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Models;
using IDWM_TallerAPI.Src.Interfaces.Repository;
using IDWM_TallerAPI.Src.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;


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


        /// Edita el perfil del usuario autenticado.
        /// "editUser" son los nuevos datos del usuario.
        /// Retorna un mensaje de éxito o error.
        [HttpPut("EditProfile")]
        public ActionResult<string> EditProfile([FromBody] EditUserDto editUser)
        {
            try{
                var userId = int.Parse(User.Claims.First(c => c.Type == "Id").Value);
                var result = _userService.EditUser(userId, editUser).Result;
                return Ok(result);   
            } catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }

        /// Cambia la contraseña del usuario autenticado.
        /// "changePasswordDto" son los datos de la contraseña nueva y de la actual.
        /// Retorna un mensaje de éxito o error.
        [HttpPut("ChangePassword")]
        public ActionResult<string> ChangeUserPassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try{
                var userId = int.Parse(User.Claims.First(c => c.Type == "Id").Value);
                var result = _userService.ChangeUserPassword(userId, changePasswordDto);
                return Ok(result);
            } catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}