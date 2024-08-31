using System.ComponentModel.DataAnnotations;

namespace ProductProject.Service.ProductService.Dto;

public class EditProductDto
{
    public string? Name { get; set; }
    public DateTime? ProduceDate { get; set; }
    [Phone]
    public string? ManufacturePhone { get; set; }
    [EmailAddress]
    public string? ManufactureEmail { get; set; }
    public bool? IsAvailable { get; set; }
}