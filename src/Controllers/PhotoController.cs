using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.DTOs;
using IDWM_TallerAPI.Src.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDWM_TallerAPI.Src.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        /// Sube una foto a Cloudinary.
        /// "photo" es el archivo de la foto a subir.
        /// Retorna los datos de la foto subida o mensaje de error.
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photo){
            var result = await _photoService.AddPhoto(photo);
            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            PhotoDto photoDto = new PhotoDto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            return Ok(photoDto);
        }
    }
}