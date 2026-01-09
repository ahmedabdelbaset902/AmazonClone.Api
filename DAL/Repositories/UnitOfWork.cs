using DAL.Contracts;
using DAL.Dbcontext;
using Domains;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AmazonCloneContext _ctx;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();
        private IDbContextTransaction? _tx;
        private readonly ILoggerFactory _loggerFactory;

        public UnitOfWork(AmazonCloneContext ctx, ILoggerFactory loggerFactory)
        {
            _ctx = ctx;
            _loggerFactory = loggerFactory;
        }

        // Get repository for any entity
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            return (IGenericRepository<T>)_repositories.GetOrAdd(
                typeof(T),
                _ => new GenericRepository<T>(
                        _ctx,
                        _loggerFactory.CreateLogger<GenericRepository<T>>()));
        }

        // Begin transaction
        public async Task BeginTransactionAsync()
        {
            _tx = await _ctx.Database.BeginTransactionAsync();
        }

        // Commit transaction
        public async Task CommitAsync()
        {
            try
            {
                await _ctx.SaveChangesAsync();
                if (_tx is not null)
                    await _tx.CommitAsync();
            }
            catch
            {
                if (_tx is not null) await _tx.RollbackAsync();
                throw;
            }
        }

        // Rollback transaction
        public async Task RollbackAsync()
        {
            if (_tx is not null)
                await _tx.RollbackAsync();
        }

        // Save changes without transaction
        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

        // Dispose
        public async ValueTask DisposeAsync()
        {
            if (_tx is not null)
            {
                await _tx.DisposeAsync();
            }
            await _ctx.DisposeAsync();
        }
    }
}
