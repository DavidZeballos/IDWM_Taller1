using System.ComponentModel.DataAnnotations;

namespace IDWM_TallerAPI.Src.Validations
{
    public class ValidBirthDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime date)
            {
                return new ValidationResult("La fecha no es válida.");
            }

            if (date >= DateTime.UtcNow)
            {
                return new ValidationResult("La fecha de nacimiento debe ser anterior a la fecha actual.");
            }

            return ValidationResult.Success;
        }
    }
}
