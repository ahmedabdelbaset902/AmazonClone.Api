using Bl.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl.Contracts
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<int> CreateProductAsync(CreateProductDto dto);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<bool> UpdateProductAsync(ProductDto dto);
        Task<bool> DeleteProductAsync(int id);
    }
}
