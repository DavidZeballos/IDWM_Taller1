using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class MakePurchaseDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe ser mayor a 0.")]
        public required int Quantity { get; set; }

    }
}