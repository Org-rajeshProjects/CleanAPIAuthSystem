using CleanAPIAuthSystem.Application.Interfaces.Repositories;
using CleanAPIAuthSystem.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Interfaces
{
    /// <summary>
    /// Unit of Work Pattern - Manages transactions across multiple repositories
    /// Theory: Unit of Work maintains a list of objects affected by a business transaction
    /// and coordinates writing out changes and resolving concurrency problems
    /// 
    /// Problem it solves: Imagine you need to:
    /// 1. Create a user
    /// 2. Create a refresh token for that user
    /// 3. Log the registration event
    /// 
    /// If step 3 fails, we want to rollback steps 1 and 2
    /// Without UoW: Each repository saves independently - partial data corruption
    /// With UoW: All changes are saved together in one transaction - all or nothing
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repository properties - Lazy loaded repositories
        /// Theory: Only create repository instances when accessed
        /// Saves memory if some repositories aren't used in a request
        /// </summary>
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }

        /// <summary>
        /// Commit all changes to database
        /// Theory: ACID transaction properties
        /// - Atomicity: All changes succeed or all fail
        /// - Consistency: Database rules are enforced
        /// - Isolation: Concurrent transactions don't interfere
        /// - Durability: Committed changes are permanent
        /// 
        /// Returns: Number of records affected
        /// </summary>
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Start explicit transaction
        /// Theory: Sometimes need manual transaction control
        /// Example: Read data, do calculations, then update
        /// Default isolation level: Read Committed (good balance)
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commit transaction
        /// Theory: Makes all changes permanent
        /// After commit, other transactions can see the changes
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rollback transaction
        /// Theory: Undo all changes since BeginTransaction
        /// Used when error occurs mid-transaction
        /// Database returns to state before transaction started
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }

}
