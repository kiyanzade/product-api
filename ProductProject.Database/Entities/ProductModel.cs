using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductAPI.Models
{
    public class ProductModel
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string? OwnerId { get; set; }

        [Required]
        public DateTime ProduceDate { get; set; }

        [Required]
        [Phone]
        public string ManufacturePhone { get; set; }

        [Required]
        [EmailAddress]
        public string ManufactureEmail { get; set; }

        public bool IsAvailable { get; set; }
    }
}
