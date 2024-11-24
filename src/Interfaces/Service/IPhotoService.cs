using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace IDWM_TallerAPI.Src.Interfaces.Service
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhoto(IFormFile photo);

        Task<DeletionResult> DeletePhoto(string publicId);
    }
}