﻿using CaseAPI.Dtos.Order;
using CaseAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using CaseAPI.Data;
using CaseAPI.Dtos.Product;
using Microsoft.AspNetCore.Authorization;
using CaseAPI.Repositories.Interfaces;

namespace CaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly CaseDbContext _context;

        public OrderController(IOrderRepository orderRepo, CaseDbContext context)
        {
            _orderRepo = orderRepo;
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderRepo.GetOrdersByUserAsync(userId);
            if (orders == null || orders.First().AppUserId != userId)
            {
                return NotFound();
            }

            var orderDtos = orders.Select(o => new ResultOrderDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                OrderDate = o.OrderDate,
                UpdatedTime = o.UpdatedTime,
                Status = o.Status.ToString(),
                TotalPrice = o.TotalPrice,
                OrderItems = o.OrderItems.Select(oi => new ResultOrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Product = new ResultProductDto
                    {
                        Id = oi.Product.Id,
                        Name = oi.Product.Name,
                        Price = oi.Product.Price,
                        ImageUrl = oi.Product.ImageUrl
                    }
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("AllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepo.GetAllAsync();

            return Ok(orders);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null || order.AppUserId != userId)
            {
                return NotFound();
            }

            var orderDto = new ResultOrderDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                UpdatedTime = order.UpdatedTime,
                Status = order.Status.ToString(),
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(oi => new ResultOrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Product = new ResultProductDto
                    {
                        Id = oi.Product.Id,
                        Name = oi.Product.Name,
                        Price = oi.Product.Price,
                        ImageUrl = oi.Product.ImageUrl
                        
                    }
                }).ToList()
            };

            return Ok(orderDto);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var order = new Order
                {
                    CustomerName = createOrderDto.CustomerName,
                    OrderDate = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    AppUserId = userId,
                    Status = OrderStatus.Bekliyor,
                    TotalPrice = createOrderDto.OrderItems.Sum(oi =>
                    {
                        // Ürünün fiyatını veritabanından çekmek için:
                        var product = _context.Products.Find(oi.ProductId);
                        return product != null ? product.Price * oi.Quantity : 0;
                    }),
                    OrderItems = createOrderDto.OrderItems.Select(oi => new OrderItem
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                    }).ToList()
                };
                if(order.TotalPrice != createOrderDto.TotalPrice)
                {
                    return BadRequest(new { description = "Total Price uyuşmazlığı var, sipariş başarısız!" });
                }
                    await _orderRepo.AddAsync(order);
                    await _orderRepo.SaveAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { description = ex.Message });
            }

            return Ok(new { message = "Sipariş Başarılıyla Oluşturuldu." });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderRepo.GetByIdAsync(updateOrderDto.Id);
            if (order == null || order.AppUserId != userId)
            {
                return NotFound();
            }

            order.UpdatedTime = DateTime.Now;
            // String değeri enum'a parse etme.
            if (Enum.TryParse<OrderStatus>(updateOrderDto.Status, out var newStatus))
            {
                order.Status = newStatus;
            }

            _orderRepo.Update(order);
            await _orderRepo.SaveAsync();

            return Ok(new { message = "Güncelleme Başarılı." });
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null || order.AppUserId != userId)
            {
                return NotFound();
            }

            _orderRepo.Delete(order);
            await _orderRepo.SaveAsync();

            return Ok(new { message = "Sipariş Başarıyla Silindi." });
        }
    }
}

