
using System.ComponentModel.DataAnnotations;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "La Contraseña es obligatoria.")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "La Contraseña es obligatoria.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])[a-zA-Z0-9]+$", ErrorMessage = "La Contraseña debe ser alfanumérica.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(20, ErrorMessage = "La contraseña debe tener a lo más 20 caracteres.")]
        public required string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public required string ConfirmPassword { get; set; }

    }
}