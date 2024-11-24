using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class VoucherDto
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int TotalPrice { get; set; }
        public List<VoucherProductDto> Products { get; set; } = [];
    }
}