using CleanAPIAuthSystem.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.DTOs.Auth
{
    /// <summary>
    /// AuthResponse - Returned after successful authentication
    /// Theory: Contains both access token (short-lived) and refresh token (long-lived)
    /// Access token goes in Authorization header, refresh token stored securely
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// JWT Access Token
        /// Theory: Stateless authentication - server doesn't store session
        /// Token contains claims (user ID, roles) and is signed cryptographically
        /// Short expiration (15 mins) limits damage if token is stolen
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh Token for getting new access tokens
        /// Theory: Long-lived (7-30 days), stored in database for revocation
        /// Should be stored in HttpOnly cookie (not localStorage) for XSS protection
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// When access token expires (Unix timestamp)
        /// Theory: Client can proactively refresh before expiration
        /// Prevents failed API calls due to expired tokens
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// User information for UI personalization
        /// Theory: Avoid extra API call to get user data
        /// </summary>
        public UserDto User { get; set; } = null!;
    }

}
