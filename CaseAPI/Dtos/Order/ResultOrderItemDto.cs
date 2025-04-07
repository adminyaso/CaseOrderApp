using CaseAPI.Dtos.Product;
using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Order
{
    public class ResultOrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Adet en az 1 olmalıdır.")]
        public int Quantity { get; set; }
        public ResultProductDto Product { get; set; }
    }
}
