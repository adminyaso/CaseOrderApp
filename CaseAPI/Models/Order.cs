namespace CaseAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime UpdatedTime { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
