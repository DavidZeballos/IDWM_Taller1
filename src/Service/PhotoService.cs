using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Helpers;
using IDWM_TallerAPI.Src.Interfaces.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace IDWM_TallerAPI.Src.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        // Sube una imagen al servicio Cloudinary.
        public async Task<ImageUploadResult> AddPhoto(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();

            var allowedFormats = new List<string> { "image/png", "image/jpeg" };
            if (!allowedFormats.Contains(photo.ContentType))
            {
                throw new InvalidOperationException("El formato de la imagen no es válido. Solo se permiten archivos .png y .jpg.");
            }
            if(photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName,stream),
                    Transformation = new Transformation()
                       .Height(500)
                       .Width(500)
                       .Crop("fill")
                       .Gravity("face"),
                    Folder = "IDWM_StoreAPI"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        // Elimina una imagen de cloudinary por su "publicId"
        public async Task<DeletionResult> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}