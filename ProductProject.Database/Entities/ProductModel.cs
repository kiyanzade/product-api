using System.ComponentModel.DataAnnotations;

namespace ProductProject.Database.Entities
{
    public class ProductModel
    {
        [Key]
        
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime ProduceDate { get; set; }

        [Required]
        [Phone]
        [StringLength(11)]
        public string ManufacturePhone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string ManufactureEmail { get; set; }

        public bool IsAvailable { get; set; }

        public string OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }
    }
}
