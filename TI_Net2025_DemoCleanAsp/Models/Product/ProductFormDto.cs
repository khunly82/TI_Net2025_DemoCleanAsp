using System.ComponentModel.DataAnnotations;

namespace TI_Net2025_DemoCleanAsp.Models.Product
{
    public class ProductFormDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
