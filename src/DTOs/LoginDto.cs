using System.ComponentModel.DataAnnotations;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "El email no tiene formato valido")]
        public required string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(20, ErrorMessage = "La contraseña no puede sobrepasar los 20 caracteres.")]
        public required string Password { get; set; }
    }
}
