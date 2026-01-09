using Bl.DTOs;
using System.Threading.Tasks;

namespace Bl.Contracts
{
    public interface ICartService
    {
        Task<CartDto> GetUserCartAsync(int userId);
        Task AddProductAsync(int userId, int productId, int quantity);
        Task UpdateQuantityAsync(int userId, int productId, int quantity);
        Task RemoveProductAsync(int userId, int productId);
    }
}
