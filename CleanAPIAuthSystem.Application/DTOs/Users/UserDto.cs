using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.DTOs.Users
{
    /// <summary>
    /// UserDto - Sanitized user data for API responses
    /// Theory: Never expose sensitive data (password hashes, etc.)
    /// DTO pattern allows returning only safe, necessary fields
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// List of linked social providers
        /// Allows UI to show which accounts are connected
        /// </summary>
        public List<string> LinkedProviders { get; set; } = new();
    }

}
