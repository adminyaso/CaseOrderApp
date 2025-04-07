using CaseAPI.Dtos.Product;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Order
{
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }

        [DefaultValue(1)]
        [Range(1, int.MaxValue, ErrorMessage = "Adet en az 1 olmalıdır.")]
        public int Quantity { get; set; }
    }
}
