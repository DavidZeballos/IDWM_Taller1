namespace IDWM_TallerAPI.Src.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public required DateTime Date { get; set; }
        public required int Quantity { get; set; }
        public required int TotalPrice { get; set; }

        public string? DeliveryAddress { get; set; }

        public User User { get; set; } = null!;
        public Product Product { get; set; } = null!;

    }
}
