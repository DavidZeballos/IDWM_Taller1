using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Validations;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener caracteres del abecedario español.")]
        [MinLength(8, ErrorMessage ="El nombre debe tener al menos 8 caracteres.")]
        [MaxLength(255, ErrorMessage = "El nombre no puede sobrepasar los 255 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene formato valido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])[a-zA-Z0-9]+$", ErrorMessage = "La Contraseña debe ser alfanumérica.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [MaxLength(20, ErrorMessage = "La contraseña no puede sobrepasar los 20 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar nuevamente la contraseña.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo rut es obligatorio")]
        [Rut(ErrorMessage = "El rut no es valido")]
        public string Rut { get; set; } = string.Empty;
        
        [DataType(DataType.Date)]
        [ValidBirthDate(ErrorMessage = "La fecha de nacimiento debe ser anterior a la fecha actual.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^(Masculino|Femenino|Prefiero no decirlo|Otro)$", ErrorMessage = "Debe ser entre Masculino, Femenino, Prefiero no decirlo, y Otro.")]
        public string Gender { get; set; } = string.Empty;



    }
}