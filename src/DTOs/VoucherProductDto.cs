using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWM_TallerAPI.Src.DTOs
{
    public class VoucherProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty; 
        public int ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int TotalProductPrice => ProductPrice * Quantity;
    }
}