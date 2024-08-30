namespace ProductProject.Service.ProductService.Dto;

public class GetProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? OwnerId { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
}