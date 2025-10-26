using CleanAPIAuthSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Interfaces.Repositories
{
    /// <summary>
    /// IUserRepository - Specific repository for User entity
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Find user by email
        /// </summary>
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Find user by email and include related data
        /// </summary>
        Task<User?> GetByEmailWithRelationsAsync(
            string email,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Find user by social login provider
        /// </summary>
        Task<User?> GetBySocialLoginAsync(
            string provider,
            string providerKey,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if username is already taken
        /// </summary>
        Task<bool> IsUserNameTakenAsync(
            string userName,
            CancellationToken cancellationToken = default);
    }
}
