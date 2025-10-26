using CleanAPIAuthSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Entities
{
    /// <summary>
    /// UserSocialLogin Entity - Links social provider accounts to users
    /// Theory: Allows users to sign in with Google, GitHub, Microsoft, etc.
    /// Stores the provider's unique ID to match accounts
    /// 
    /// Inherits from BaseEntity<Guid> to get Id, CreatedAt, UpdatedAt
    /// </summary>
    public class UserSocialLogin : BaseEntity<Guid>
    {
        /// <summary>
        /// Foreign Key to User
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Provider name (Google, GitHub, Microsoft)
        /// Theory: Identifies which OAuth provider this login is for
        /// Used to construct the OAuth flow URL
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Provider's unique user ID
        /// Theory: Each OAuth provider assigns a unique ID to users
        /// We store this to match future logins to the same account
        /// Example: Google's "sub" claim, GitHub's user ID
        /// </summary>
        public string ProviderKey { get; set; } = string.Empty;

        /// <summary>
        /// Optional: Store provider-specific data
        /// Theory: JSON field allows storing flexible data without schema changes
        /// Can store profile picture URL, provider username, etc.
        /// </summary>
        public string? ProviderData { get; set; }

        // CreatedAt is inherited from BaseEntity<Guid>
        // No need to redeclare here

        /// <summary>
        /// Navigation property back to User
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}
