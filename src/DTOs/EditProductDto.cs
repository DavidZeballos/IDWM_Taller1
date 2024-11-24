using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDWM_TallerAPI.Src.Models;

using System.ComponentModel.DataAnnotations;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class EditProductDto
    {
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener caracteres alfabéticos.")]
        [MinLength(10, ErrorMessage ="El nombre debe tener al menos 10 caracteres.")]
        [MaxLength(64, ErrorMessage = "El nombre no puede sobrepasar los 64 caracteres.")]
        
        public string? Name { get; set; }
        [Range(1, 99999999, ErrorMessage = "El precio debe ser un número entero positivo menor que 100 millones.")]
        public int? Price { get; set; }

        [Range(0, 99999, ErrorMessage = "La cantidad en stock debe ser un número no negativo menor que 100 mil.")]
        public int? InStock { get; set; }

        [RegularExpression(@"^.*\.(jpg|png)$", ErrorMessage = "La imagen debe ser de formato .png o .jpg.")]
        public string? ImageURL { get; set; }

        [RegularExpression(@"^(Poleras|Gorros|Juguetería|Alimentación|Libros)$", 
            ErrorMessage = "El tipo debe ser: Poleras, Gorros, Juguetería, Alimentación o Libros.")]
        public int? ProductTypeId { get; set; }
        public ProductTypeDto? ProductType { get; set; }

    }
}