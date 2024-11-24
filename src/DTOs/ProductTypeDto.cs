using System.ComponentModel.DataAnnotations;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class ProductTypeDto
    {
        public int Id { get; set; }

        [RegularExpression(@"^(Poleras|Gorros|Juguetería|Alimentación|Libros)$", 
            ErrorMessage = "El tipo debe ser: Poleras, Gorros, Juguetería, Alimentación o Libros.")]
        public string Name { get; set; } = string.Empty;
    }
}