using Domains;
using System;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // يجيب Repository لأي Entity
        IGenericRepository<T> Repository<T>() where T : BaseEntity;

        // Transaction management
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        // Save changes بدون Transaction
        Task<int> SaveChangesAsync();
    }
}
