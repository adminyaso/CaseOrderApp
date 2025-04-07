using CaseAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
namespace CaseAPI.Data

{
    public class CaseDbContext : IdentityDbContext<AppUser>
    {
        public CaseDbContext(DbContextOptions<CaseDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CaseDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
