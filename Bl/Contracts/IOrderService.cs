using Bl.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl.Contracts
{
    public interface IOrderService
    {
        Task<int> CheckoutAsync(int userId);
        Task<List<OrderDto>> GetOrdersForUserAsync(int userId);
    }
}
