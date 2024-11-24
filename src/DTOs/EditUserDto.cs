using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class EditUserDto
    {
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [RegularExpression(@"^(Masculino|Femenino|Prefiero no decirlo|Otro)$", ErrorMessage = "Debe ser entre: Masculino, Femenino, Prefiero no decirlo u Otro.")]
        public string? Gender { get; set; }
    }
}