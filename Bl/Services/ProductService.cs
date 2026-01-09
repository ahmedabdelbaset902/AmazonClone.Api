using Bl.Contracts;
using Bl.DTOs;
using AutoMapper;
using DAL.Contracts;
using Domains;
using Microsoft.Extensions.Logging; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ProductService> logger) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Fetching all products...");
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();

            var result = products
                .Select(p => _mapper.Map<Product, ProductDto>(p))
                .ToList();

            _logger.LogInformation($"Fetched {result.Count} products.");
            return result;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching product with ID {id}...");
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return null;
            }

            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<int> CreateProductAsync(CreateProductDto dto)
        {
            _logger.LogInformation($"Creating a new product: {dto.Name}...");

            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(dto.CategoryId);
            if (category == null)
            {
                _logger.LogError($"Category with ID {dto.CategoryId} does not exist.");
                throw new Exception($"Category with ID {dto.CategoryId} does not exist.");
            }

            var product = _mapper.Map<CreateProductDto, Product>(dto);

            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Product created with ID {product.Id}.");
            return product.Id;
        }

        public async Task<bool> UpdateProductAsync(ProductDto dto)
        {
            _logger.LogInformation($"Updating product with ID {dto.Id}...");
            var product = _mapper.Map<ProductDto, Product>(dto);
            var result = await _unitOfWork.Repository<Product>().UpdateAsync(product);

            if (result)
                _logger.LogInformation($"Product with ID {dto.Id} updated successfully.");
            else
                _logger.LogWarning($"Failed to update product with ID {dto.Id}.");

            return result;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            _logger.LogInformation($"Deleting product with ID {id}...");
            var result = await _unitOfWork.Repository<Product>().DeleteAsync(id);

            if (result)
                _logger.LogInformation($"Product with ID {id} deleted successfully.");
            else
                _logger.LogWarning($"Failed to delete product with ID {id}.");

            return result;
        }
    }
}
