using Bl.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCategoryDto dto);
        Task<bool> UpdateAsync(CategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
