using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductProject.Service.ProductService.Dto
{
    public class AddProductDto
    {
        [Required]
        public string Name { get; set; }
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
