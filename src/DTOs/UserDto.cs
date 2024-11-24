using System.ComponentModel.DataAnnotations;
using IDWM_TallerAPI.Src.Validations;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        [MinLength(8, ErrorMessage ="El nombre debe tener al menos 8 caracteres.")]
        [MaxLength(255, ErrorMessage = "El nombre no puede sobrepasar los 255 caracteres.")]
        public required string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "El email no tiene formato valido")]
        public required string Email { get; set; } = string.Empty;

        [Required]
        [Rut(ErrorMessage = "El rut no es valido")]
        public required string Rut { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Date)]
        [ValidBirthDate(ErrorMessage = "La fecha de nacimiento debe ser anterior a la fecha actual.")]
        public required DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^(Masculino|Femenino|Prefiero no decirlo|Otro)$", ErrorMessage = "Debe ser entre Masculino, Femenino, Prefiero no decirlo, y Otro.")]
        public required string Gender { get; set; } = string.Empty;
        public required string NameRol { get; set; } = string.Empty;
        public List<ProductDto> Products { get; set; } = [];
    }
}
