using CleanAPIAuthSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Interfaces.Repositories
{
    /// <summary>
    /// IRefreshTokenRepository - Repository for refresh token operations
    /// </summary>
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        /// <summary>
        /// Get refresh token with user data
        /// Theory: When validating refresh token, we need user info too
        /// Eager load user to avoid extra query
        /// </summary>
        Task<RefreshToken?> GetByTokenWithUserAsync(
            string token,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all active tokens for a user
        /// Theory: Used when logging out all devices
        /// Active = not revoked and not expired
        /// </summary>
        Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revoke all tokens for a user
        /// Theory: Security feature - logout from all devices
        /// Useful when user suspects account compromise
        /// </summary>
        Task RevokeAllUserTokensAsync(
            Guid userId,
            string revokedByIp,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Clean up expired tokens (maintenance task)
        /// Theory: Expired tokens are useless, delete them to save space
        /// Run this periodically (daily cron job)
        /// </summary>
        Task DeleteExpiredTokensAsync(CancellationToken cancellationToken = default);
    }
}
