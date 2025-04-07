using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseAPI.Data;
using CaseAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

public static class DataSeeder
{
    public static async Task SeedAsync(CaseDbContext context, UserManager<AppUser> userManager)
    {
        await context.Database.MigrateAsync();

        var rnd = new Random();

            var products = new List<Product>();
            for (int i = 1; i <= 20; i++)
            {
                products.Add(new Product
                {
                    Name = $"Product {i} - {Guid.NewGuid().ToString().Substring(0, 5)}",
                    Price = Math.Round((decimal)(rnd.Next(100, 1000) / 10.0), 2),
                    ImageUrl = "https://artjewellerywatches.com/cdn/shop/articles/service.jpg?v=1707254518&width=1800"
                });
            }
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

        var testUser = await userManager.FindByNameAsync("test1234");
        if (testUser == null)
        {
            testUser = new AppUser { UserName = "test1234"};
            var createResult = await userManager.CreateAsync(testUser, "test1234");
            if (!createResult.Succeeded)
            {
                throw new Exception("Test user oluşturulamadı: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }
            var roleResult = await userManager.AddToRoleAsync(testUser, "Admin");
            if (!roleResult.Succeeded)
            {
                throw new Exception($"User rolü verilemedi: {string.Join(", ", values: roleResult.Errors.Select(e => e.Description))}");
            }
        }

            var productList = await context.Products.ToListAsync();

            for (int orderIndex = 0; orderIndex < 10; orderIndex++)
            {
                int itemCount = rnd.Next(1, 21);
                var orderItems = new List<OrderItem>();
                decimal totalPrice = 0;
                for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
                {
                    var product = productList[rnd.Next(productList.Count)];
                    int quantity = rnd.Next(1, 6);
                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = quantity
                    });
                    totalPrice += product.Price * quantity;
                }

                DateTime orderDate = DateTime.Now.AddDays(-rnd.Next(0, 30));

                var order = new Order
                {
                    CustomerName = testUser.UserName,
                    OrderDate = orderDate,
                    UpdatedTime = DateTime.Now,
                    AppUserId = testUser.Id,
                    Status = (OrderStatus)rnd.Next(0, 3),
                    TotalPrice = totalPrice,
                    OrderItems = orderItems
                };

                context.Orders.Add(order);
            }

            await context.SaveChangesAsync();
    }
}
