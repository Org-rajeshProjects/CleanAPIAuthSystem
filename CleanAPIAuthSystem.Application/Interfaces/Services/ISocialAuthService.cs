using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Interfaces.Services
{
    /// <summary>
    /// Social authentication service interface
    /// Theory: OAuth 2.0 integration with external providers
    /// 
    /// OAuth 2.0 Authorization Code Flow:
    /// 1. User clicks "Login with Google"
    /// 2. Frontend redirects to Google's authorization page
    /// 3. User approves permission request
    /// 4. Google redirects back with authorization code
    /// 5. Frontend sends code to our backend (this service)
    /// 6. Backend exchanges code for access token (server-to-server)
    /// 7. Backend uses access token to get user profile
    /// 8. Backend creates/finds user in our database
    /// 9. Backend returns our JWT tokens to frontend
    /// 
    /// Why Authorization Code Flow (not Implicit Flow)?
    /// - More secure: Access token never exposed to browser
    /// - Client secret used in server-to-server exchange
    /// - Refresh token support
    /// - Industry standard for web apps
    /// </summary>
    public interface ISocialAuthService
    {
        /// <summary>
        /// Exchange authorization code for user info
        /// Theory: Completes OAuth flow by:
        /// 1. Exchanging code for access token (server-to-server, secure)
        /// 2. Using access token to fetch user profile from provider
        /// 3. Normalizing provider-specific response to common format
        /// 
        /// Why server-side exchange?
        /// - Keeps client secret secure (never sent to browser)
        /// - Validates redirect URI (prevents authorization code interception)
        /// - Can refresh tokens later if needed
        /// 
        /// Provider-specific differences:
        /// - Google: Returns given_name, family_name
        /// - GitHub: Returns name (full name), login (username)
        /// - Microsoft: Returns givenName, surname
        /// 
        /// This method abstracts those differences
        /// </summary>
        /// <param name="provider">Provider name: "Google", "GitHub", or "Microsoft"</param>
        /// <param name="code">Authorization code from OAuth provider</param>
        /// <param name="redirectUri">Redirect URI (must match OAuth config)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Normalized user info or null if authentication failed</returns>
        Task<SocialUserInfo?> GetUserInfoAsync(
            string provider,
            string code,
            string redirectUri,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Social user information DTO
    /// Theory: Normalized user data from different providers
    /// 
    /// Why normalize?
    /// - Each provider returns different JSON structure
    /// - Different field names (given_name vs givenName vs name)
    /// - Optional fields vary by provider
    /// - Our application needs consistent data structure
    /// 
    /// Normalization strategy:
    /// - Id: Provider's unique user identifier
    /// - Email: Always required (we request email scope)
    /// - FirstName/LastName: Parse from provider data (may be null)
    /// - ProfilePictureUrl: Optional, different APIs per provider
    /// - Provider: Track which provider this data came from
    /// </summary>
    public class SocialUserInfo
    {
        /// <summary>
        /// Provider's unique user ID
        /// Examples:
        /// - Google: "123456789012345678901"
        /// - GitHub: "12345678"
        /// - Microsoft: "a1b2c3d4-e5f6-g7h8-i9j0-k1l2m3n4o5p6"
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// User's email address
        /// Theory: All providers support email scope
        /// We require email for user identification
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// First name (given name)
        /// May be null if provider doesn't return it
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name (family name, surname)
        /// May be null if provider doesn't return it
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Profile picture URL
        /// Optional: Not all providers return this
        /// Google/GitHub provide it, but requires additional API call for Microsoft
        /// </summary>
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// Provider name for tracking
        /// Values: "Google", "GitHub", "Microsoft"
        /// </summary>
        public string Provider { get; set; } = string.Empty;
    }
}
