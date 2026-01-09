using AutoMapper;
using Bl.Contracts;
using Bl.DTOs;
using DAL.Contracts;
using Domains;
using Microsoft.Extensions.Logging; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger; 

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all categories...");
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();

            var result = categories
                .Select(c => _mapper.Map<Category, CategoryDto>(c))
                .ToList();

            _logger.LogInformation($"Fetched {result.Count} categories.");
            return result;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching category with ID {id}...");
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found.");
                return null;
            }

            return _mapper.Map<Category, CategoryDto>(category);
        }

        public async Task<int> CreateAsync(CreateCategoryDto dto)
        {
            _logger.LogInformation($"Creating new category: {dto.Name}...");

            if (dto.ParentCategoryId.HasValue)
            {
                var parent = await _unitOfWork
                    .Repository<Category>()
                    .GetByIdAsync(dto.ParentCategoryId.Value);

                if (parent == null)
                {
                    _logger.LogError($"Parent category with ID {dto.ParentCategoryId.Value} does not exist.");
                    throw new Exception("Parent category does not exist");
                }
            }

            var category = _mapper.Map<CreateCategoryDto, Category>(dto);

            await _unitOfWork.Repository<Category>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Category created with ID {category.Id}.");
            return category.Id;
        }

        public async Task<bool> UpdateAsync(CategoryDto dto)
        {
            _logger.LogInformation($"Updating category with ID {dto.Id}...");
            var category = _mapper.Map<CategoryDto, Category>(dto);
            var result = await _unitOfWork.Repository<Category>().UpdateAsync(category);

            if (result)
                _logger.LogInformation($"Category with ID {dto.Id} updated successfully.");
            else
                _logger.LogWarning($"Failed to update category with ID {dto.Id}.");

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting category with ID {id}...");
            var result = await _unitOfWork.Repository<Category>().DeleteAsync(id);

            if (result)
                _logger.LogInformation($"Category with ID {id} deleted successfully.");
            else
                _logger.LogWarning($"Failed to delete category with ID {id}.");

            return result;
        }
    }
}
