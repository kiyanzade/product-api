using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductAPI.Models
{
    public class ProductModel
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
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
