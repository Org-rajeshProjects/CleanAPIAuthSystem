using CleanAPIAuthSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Interfaces.Repositories
{
    /// <summary>
    /// Generic Repository Interface - Master repository that all repositories inherit from
    /// Theory: Repository Pattern abstracts data access logic
    /// </summary>
    /// <typeparam name="T">Entity type (User, RefreshToken, etc.)</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get entity by ID
        /// </summary>
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Find entities matching a condition
        /// </summary>
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Find first entity matching condition or null
        /// </summary>
        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new entity
        /// </summary>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add multiple entities
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update existing entity
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Update multiple entities
        /// </summary>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        void Remove(T entity);

        /// <summary>
        /// Delete multiple entities
        /// </summary>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Check if any entity matches condition
        /// </summary>
        Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Count entities matching condition
        /// </summary>
        Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get paged results
        /// </summary>
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);
    }

}
