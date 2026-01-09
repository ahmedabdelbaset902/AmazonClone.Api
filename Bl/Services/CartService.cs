using AutoMapper;
using Bl.Contracts;
using Bl.DTOs;
using DAL.Contracts;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // <-- for ILogger
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger; // <-- logger injected

        public CartService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        // ================= Get User Cart =================
        public async Task<CartDto> GetUserCartAsync(int userId)
        {
            _logger.LogInformation($"Fetching cart for user {userId}.");
            var cart = await GetOrCreateCart(userId);
            return _mapper.Map<Cart, CartDto>(cart);
        }

        // ================= Add Product =================
        public async Task AddProductAsync(int userId, int productId, int quantity)
        {
            _logger.LogInformation($"Adding product {productId} (Qty: {quantity}) to cart for user {userId}.");
            var cart = await GetOrCreateCart(userId);

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
                _logger.LogInformation($"Updated quantity for product {productId} to {item.Quantity}.");
            }
            else
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogError($"Product with ID {productId} not found.");
                    throw new Exception($"Product with ID {productId} not found");
                }

                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Product = product
                });

                _logger.LogInformation($"Product {productId} added to cart.");
            }

            await _unitOfWork.SaveChangesAsync();
        }

        // ================= Update Quantity =================
        public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            _logger.LogInformation($"Updating quantity for product {productId} in cart for user {userId} to {quantity}.");
            var cart = await GetOrCreateCart(userId);

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                _logger.LogWarning($"Product {productId} not found in cart for user {userId}.");
                return;
            }

            item.Quantity = quantity;
            await _unitOfWork.SaveChangesAsync();
        }

        // ================= Remove Product =================
        public async Task RemoveProductAsync(int userId, int productId)
        {
            _logger.LogInformation($"Removing product {productId} from cart for user {userId}.");
            var cart = await GetOrCreateCart(userId);

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                _logger.LogWarning($"Product {productId} not found in cart for user {userId}.");
                return;
            }

            cart.Items.Remove(item);
            _logger.LogInformation($"Product {productId} removed from cart.");
            await _unitOfWork.SaveChangesAsync();
        }

        // ================= Private Helper =================
        private async Task<Cart> GetOrCreateCart(int userId)
        {
            var cart = await _unitOfWork.Repository<Cart>()
                .GetAllQueryable()
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null) return cart;

            _logger.LogInformation($"Creating new cart for user {userId}.");
            cart = new Cart { UserId = userId };
            await _unitOfWork.Repository<Cart>().AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return cart;
        }
    }
}
