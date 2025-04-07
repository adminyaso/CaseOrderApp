using CaseAPI.Models;

namespace CaseAPI.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);

        Task<Order> GetOrderByIdAsync(int id);
    }
}
