namespace IDWM_TallerAPI.Src.DTOs
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public required DateTime Date { get; set; }
        public required int Quantity { get; set; }
        public required int TotalPrice { get; set; }

        public string? DeliveryAddress { get; set; }

        public required int UserId { get; set; }
        public UserDto User { get; set; } = null!;
        public required int ProductId { get; set; }
        public ProductDto Product { get; set; } = null!;

    }
}
