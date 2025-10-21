using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Entities
{
    /// <summary>
    /// RefreshToken Entity - Stores long-lived tokens for renewing access tokens
    /// Theory: Access tokens are short-lived (15 mins) for security
    /// Refresh tokens are long-lived (7-30 days) and allow getting new access tokens
    /// without re-authentication
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign Key - Links token to a user
        /// Theory: Establishes the relationship between RefreshToken and User
        /// EF Core uses this to create the database foreign key constraint
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The actual token string - cryptographically random
        /// Theory: Should be at least 256 bits of randomness to prevent guessing
        /// Generated using cryptographically secure random number generator (CSPRNG)
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration date
        /// Theory: Limits the window of vulnerability if token is stolen
        /// Typically set to 7-30 days depending on security requirements
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Flag to manually invalidate tokens
        /// Theory: Allows user logout and token revocation for security
        /// When user logs out, we mark their tokens as revoked
        /// </summary>
        public bool IsRevoked { get; set; }

        /// <summary>
        /// When token was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// IP address that created this token
        /// Theory: Helps detect suspicious activity (token used from different location)
        /// Part of defense-in-depth security strategy
        /// </summary>
        public string? CreatedByIp { get; set; }

        /// <summary>
        /// Tracks when and how token was revoked
        /// Theory: Audit trail for security investigations
        /// </summary>
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }

        /// <summary>
        /// If this token was replaced by a new one, store the new token
        /// Theory: Token rotation - when refreshing, create new token and invalidate old one
        /// Helps detect token theft (if old token is reused, we know it's compromised)
        /// </summary>
        public string? ReplacedByToken { get; set; }

        /// <summary>
        /// Navigation property back to User
        /// Theory: Two-way navigation allows querying from either direction
        /// User.RefreshTokens or RefreshToken.User
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Helper property - Check if token is still valid
        /// Theory: Encapsulates business logic in the entity (Domain-Driven Design)
        /// </summary>
        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    }

}
