using System.ComponentModel.DataAnnotations;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage = "El nombre de producto es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener caracteres alfabéticos.")]
        [MinLength(10, ErrorMessage ="El nombre debe tener al menos 10 caracteres.")]
        [MaxLength(64, ErrorMessage = "El nombre no puede sobrepasar los 64 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El precio del producto es obligatorio.")]
        [Range(1, 99999999, ErrorMessage = "El precio debe ser un número entero positivo menor que 100 millones.")]
        public required int Price { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
        [Range(0, 99999, ErrorMessage = "La cantidad en stock debe ser un número no negativo menor que 100 mil.")]
        public required int InStock { get; set; }

        [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
        [RegularExpression(@"^.*\.(jpg|png)$", ErrorMessage = "La imagen debe ser de formato .png o .jpg.")]
        public required string ImageURL { get; set; }

        [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
        [RegularExpression(@"^(Poleras|Gorros|Juguetería|Alimentación|Libros)$", 
            ErrorMessage = "El tipo debe ser: Poleras, Gorros, Juguetería, Alimentación o Libros.")]

        public required int ProductTypeId { get; set; }
        public required ProductTypeDto ProductType { get; set; } = null!;
    }
}