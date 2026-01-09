using AutoMapper;
using Bl.Contracts;
using Bl.DTOs;
using DAL.Contracts;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> CheckoutAsync(int userId)
    {
        _logger.LogInformation($"User {userId} is attempting to checkout.");

        // Get the cart for the user
        var cart = await _unitOfWork.Repository<Cart>()
            .GetAllQueryable()
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
        {
            _logger.LogWarning($"User {userId} tried to checkout an empty cart.");
            throw new Exception("Cart is empty");
        }

        // Map CartItems → OrderItems
        var orderItems = _mapper.Map<List<OrderItem>>(cart.Items);

        // Create the order
        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.Now,
            Items = orderItems,
            TotalAmount = orderItems.Sum(i => i.Price * i.Quantity),
            Status = OrderStatus.Pending
        };

        // Add the order
        await _unitOfWork.Repository<Order>().AddAsync(order);

        // Clear the cart
        cart.Items.Clear();

        // Save changes
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation($"Order {order.Id} created successfully for user {userId} with total amount {order.TotalAmount}.");

        return order.Id;
    }

    public async Task<List<OrderDto>> GetOrdersForUserAsync(int userId)
    {
        _logger.LogInformation($"Fetching orders for user {userId}.");

        var orders = await _unitOfWork.Repository<Order>().GetAllAsync();

        var userOrders = orders.Where(o => o.UserId == userId).ToList();

        var result = _mapper.Map<List<OrderDto>>(userOrders);

        _logger.LogInformation($"Fetched {result.Count} orders for user {userId}.");

        return result;
    }
}
