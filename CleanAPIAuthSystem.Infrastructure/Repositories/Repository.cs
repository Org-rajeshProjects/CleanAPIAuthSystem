using CleanAPIAuthSystem.Application.Interfaces.Repositories;
using CleanAPIAuthSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Generic Repository Implementation - Base repository all others inherit from
    /// Theory: Repository Pattern implementation using EF Core
    /// Provides common CRUD operations for all entities
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Database context
        /// Theory: Protected allows derived classes to access
        /// Each repository gets the same DbContext instance (scoped lifetime)
        /// </summary>
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// DbSet for this entity type
        /// Theory: DbSet<T> represents the table in the database
        /// All queries start from this DbSet
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor - receives DbContext via DI
        /// Theory: Constructor injection ensures repository always has valid context
        /// </summary>
        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>(); // Get DbSet for entity type T
        }

        /// <summary>
        /// Get entity by ID
        /// Theory: FindAsync is optimized for primary key lookups
        /// Checks change tracker first (already loaded entities)
        /// Then queries database if not found
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// Get all entities
        /// Theory: ToListAsync materializes query and returns list
        /// Warning: Don't use on large tables (loads everything into memory)
        /// For large tables, use pagination or streaming
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Find entities matching predicate
        /// Theory: Where creates IQueryable (lazy evaluation)
        /// Query not executed until ToListAsync
        /// Allows chaining: Where().OrderBy().Take()
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Find first matching entity
        /// Theory: FirstOrDefaultAsync executes query and returns first result
        /// Returns null if no match
        /// More efficient than Find().FirstOrDefault() - database returns 1 row
        /// </summary>
        public virtual async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// Add entity
        /// Theory: AddAsync adds to change tracker
        /// NOT saved to database until SaveChangesAsync
        /// Returns added entity (useful to get generated ID after save)
        /// </summary>
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <summary>
        /// Add multiple entities
        /// Theory: More efficient than multiple AddAsync calls
        /// EF Core batches into single INSERT statement
        /// </summary>
        public virtual async Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// Update entity
        /// Theory: Marks entity as Modified in change tracker
        /// EF Core tracks which properties changed
        /// Generates UPDATE only for changed columns
        /// </summary>
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Update multiple entities
        /// </summary>
        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        /// <summary>
        /// Delete entity
        /// Theory: Marks entity as Deleted in change tracker
        /// Generates DELETE statement when SaveChanges is called
        /// Note: If entity implements ISoftDeletable, DbContext intercepts
        /// and converts to UPDATE instead of DELETE
        /// </summary>
        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete multiple entities
        /// </summary>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Check if any entity matches predicate
        /// Theory: AnyAsync is more efficient than Count > 0
        /// Database stops searching after finding first match
        /// Returns true/false without counting all matches
        /// </summary>
        public virtual async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// Count entities
        /// Theory: CountAsync executes COUNT(*) query in database
        /// Returns integer count without loading entities into memory
        /// </summary>
        public virtual async Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate == null)
                return await _dbSet.CountAsync(cancellationToken);

            return await _dbSet.CountAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// Get paged results
        /// Theory: Pagination pattern
        /// Skip = (pageNumber - 1) * pageSize
        /// Take = pageSize
        /// Returns both items and total count for UI (showing "Page 1 of 10")
        /// </summary>
        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            // Validate page parameters
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Build query
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Get paged items
            // Theory: Skip and Take translate to OFFSET and FETCH in SQL
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
