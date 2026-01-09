using DAL.Contracts;
using DAL.Dbcontext;
using Domains;
using DAL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AmazonCloneContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(AmazonCloneContext context, ILogger<GenericRepository<T>> log)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = log;
        }

        // Get all
        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, $"Error in GetAllAsync for {typeof(T).Name}", _logger);
            }
        }

        // Get by Id
        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, $"Error in GetByIdAsync for {typeof(T).Name}", _logger);
            }
        }

        // Add
        public async Task<int> AddAsync(T entity)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                // اطبع الرسالة الحقيقية من قاعدة البيانات
                Console.WriteLine("InnerException: " + ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }


        // Update
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                var dbData = await GetByIdAsync(entity.Id);
                if (dbData == null) return false;

                entity.CreatedAt = dbData.CreatedAt; // keep original CreatedAt
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, $"Error in UpdateAsync for {typeof(T).Name}", _logger);
            }
        }

        // Delete
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null) return false;

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, $"Error in DeleteAsync for {typeof(T).Name}", _logger);
            }
        }

        // Change Status
        public async Task<bool> ChangeStatusAsync(int id, int userId, int status = 1)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null) return false;

                entity.Status = status;
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, $"Error in ChangeStatusAsync for {typeof(T).Name}", _logger);
            }
        }

        // Get first or default
        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, $"Error in GetFirstOrDefaultAsync for {typeof(T).Name}", _logger);
            }
        }

        // Get list by filter
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.AsNoTracking().Where(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, $"Error in GetListAsync for {typeof(T).Name}", _logger);
            }
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbSet.AsQueryable();
        }

    }
}
