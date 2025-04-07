using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Order
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Sipariş item gereklidir.")]
        [MinLength(1, ErrorMessage = "En az bir sipariş item eklenmelidir.")]
        public List<CreateOrderItemDto> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
