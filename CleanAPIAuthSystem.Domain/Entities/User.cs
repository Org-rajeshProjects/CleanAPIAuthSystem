using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Domain.Entities
{
    /// <summary>
    /// User Entity - Represents a user in the system
    /// Theory: An entity is a domain object with a unique identity that persists over time
    /// Unlike Value Objects, entities are defined by their ID, not their attributes
    /// </summary>
    public class User
    {
        /// <summary>
        /// Primary Key - Uniquely identifies each user
        /// Theory: Using GUID instead of int for security (prevents enumeration attacks)
        /// and distributed system compatibility (no conflicts when merging databases)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Email address - Used for authentication and communication
        /// Theory: Emails should be unique and serve as a natural key for users
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Username - Display name for the user
        /// Theory: Separate from email to allow users to change email without losing identity
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password - NEVER store plain text passwords
        /// Theory: We use one-way hashing (bcrypt/PBKDF2) to protect passwords
        /// Even if database is compromised, passwords remain secure
        /// Null for social login users who don't have a password
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// First and Last names for personalization
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Email verification status
        /// Theory: Two-factor verification ensures the email belongs to the user
        /// Prevents fake account creation and improves security
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Account status - Allows administrators to disable accounts
        /// Theory: Soft delete pattern - don't physically delete user data
        /// for audit trails and regulatory compliance (GDPR, etc.)
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Audit fields - Track when records are created/modified
        /// Theory: Essential for debugging, compliance, and data integrity
        /// DateTime.UtcNow ensures consistency across time zones
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property - One user can have many refresh tokens
        /// Theory: EF Core uses this for relationship mapping (1-to-many)
        /// Virtual keyword enables lazy loading (load tokens only when accessed)
        /// </summary>
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        /// <summary>
        /// Navigation property - One user can have many social logins
        /// Theory: Allows users to link multiple social accounts (Google, GitHub, etc.)
        /// </summary>
        public virtual ICollection<UserSocialLogin> SocialLogins { get; set; } = new List<UserSocialLogin>();
    }
}
