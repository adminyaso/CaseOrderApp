using CaseAPI.Dtos.Product;
using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Order
{
    public class ResultOrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Sipariş item gereklidir.")]
        [MinLength(1, ErrorMessage = "En az bir sipariş item eklenmelidir.")]
        public List<ResultOrderItemDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
