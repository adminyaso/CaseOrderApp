using CaseAPI.Data;
using CaseAPI.Models;
using CaseAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace CaseAPI.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(CaseDbContext context) : base(context) { }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == id)
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.AppUserId == userId)
                .ToListAsync();
        }
    }
}
