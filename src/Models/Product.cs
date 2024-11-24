namespace IDWM_TallerAPI.Src.Models
{
public class Product{

    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Price { get; set; }
    public int InStock { get; set; } = 0;
    public required string ImageURL { get; set; }

    //Relaciones
    public required int ProductTypeId { get; set; }
    public ProductType ProductType { get; set; } = null!;

    }
}