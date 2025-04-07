using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Product
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        [StringLength(100, ErrorMessage = "Ürün adı 100 karakterden uzun olamaz.")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat pozitif bir değer olmalıdır.")]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
