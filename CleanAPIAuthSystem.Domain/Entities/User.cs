using CleanAPIAuthSystem.Domain.Common;
using CleanAPIAuthSystem.Domain.Interfaces;
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
    /// 
    /// Inheritance from BaseEntity<Guid>:
    /// - Gets Id property of type Guid
    /// - Gets CreatedAt and UpdatedAt audit fields
    /// - Implements IEntity<Guid> for generic repository support
    /// </summary>
    public class User : BaseEntity<Guid>, IAuditableEntity
    {
        /// <summary>
        /// Primary Key inherited from BaseEntity<Guid>
        /// Theory: BaseEntity provides common properties for all entities
        /// Using GUID instead of int for security (prevents enumeration attacks)
        /// and distributed system compatibility (no conflicts when merging databases)
        /// 
        /// Note: Id property is inherited, we override it here for XML documentation
        /// </summary>
        public override Guid Id { get; set; }

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

        // CreatedAt and UpdatedAt are inherited from BaseEntity<Guid>
        // We don't need to redeclare them here

        /// <summary>
        /// IAuditableEntity implementation - who created/modified the record
        /// Theory: Track which admin/system user made changes
        /// Useful for audit trails and compliance
        /// </summary>
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

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
