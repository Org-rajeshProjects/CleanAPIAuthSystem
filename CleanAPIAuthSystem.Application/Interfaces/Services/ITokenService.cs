using CleanAPIAuthSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Interfaces.Services
{
    /// <summary>
    /// ITokenService - JWT token generation and validation
    /// Theory: Single Responsibility Principle
    /// Token logic is complex, separate from auth logic
    /// 
    /// Why separate from AuthService?
    /// - Token generation/validation is independent concern
    /// - Can be reused by other services
    /// - Easier to test in isolation
    /// - Can swap implementations (e.g., different algorithms)
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generate JWT access token
        /// Theory: JWT structure - Header.Payload.Signature
        /// 
        /// Header (Base64Url encoded):
        /// {
        ///   "alg": "HS256",  // Algorithm: HMAC SHA-256
        ///   "typ": "JWT"     // Type: JSON Web Token
        /// }
        /// 
        /// Payload (Base64Url encoded):
        /// {
        ///   "sub": "user-guid",        // Subject: User ID
        ///   "email": "user@test.com",  // User email
        ///   "exp": 1234567890,         // Expiration: Unix timestamp
        ///   "iat": 1234567890,         // Issued at: Unix timestamp
        ///   "iss": "CleanAuthSystem",  // Issuer: Who created token
        ///   "aud": "CleanAuthSystemUsers" // Audience: Who token is for
        /// }
        /// 
        /// Signature:
        /// HMACSHA256(
        ///   base64UrlEncode(header) + "." + base64UrlEncode(payload),
        ///   secret-key
        /// )
        /// 
        /// Final Token: header.payload.signature
        /// Example: eyJhbGc...IUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWI...MTIzNDU2Nzg5MCIsIm5hbWUiOi.TJVA95Or...M7E2cBab30RM
        /// </summary>
        /// <param name="user">User entity containing claims data</param>
        /// <returns>JWT token string</returns>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generate refresh token
        /// Theory: Refresh token is NOT a JWT
        /// It's a cryptographically secure random string stored in database
        /// 
        /// Why not JWT?
        /// - JWTs can't be revoked (stateless)
        /// - Refresh tokens need revocation for logout
        /// - Random string is simpler and more secure
        /// - Database storage allows audit trail
        /// 
        /// Generation:
        /// 1. Generate 64 bytes (512 bits) of cryptographically secure random data
        /// 2. Encode as Base64 string
        /// 3. Store in database with user ID, expiration, IP address
        /// </summary>
        /// <param name="ipAddress">IP address creating the token (for audit)</param>
        /// <returns>RefreshToken entity ready to be saved to database</returns>
        RefreshToken GenerateRefreshToken(string ipAddress);

        /// <summary>
        /// Validate JWT token and extract claims
        /// Theory: Validates multiple aspects of the token
        /// 
        /// Validation checks:
        /// 1. Signature verification (token not tampered with)
        /// 2. Expiration check (token not expired)
        /// 3. Issuer validation (token from our server)
        /// 4. Audience validation (token for our application)
        /// 5. Algorithm validation (prevent algorithm substitution attacks)
        /// 
        /// ClaimsPrincipal:
        /// - Represents the user's identity
        /// - Contains all claims from JWT payload
        /// - Used by [Authorize] attribute to identify user
        /// - Available in controllers via User property
        /// 
        /// Example usage in controller:
        /// var userId = User.FindFirst("sub")?.Value;
        /// var email = User.FindFirst("email")?.Value;
        /// </summary>
        /// <param name="token">JWT token string</param>
        /// <returns>ClaimsPrincipal if valid, null if invalid/expired</returns>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Get user ID from JWT token
        /// Theory: Extract "sub" (subject) claim from token
        /// 
        /// "sub" is a standard JWT claim defined in RFC 7519
        /// It should contain a unique identifier for the user
        /// 
        /// Why use this instead of ValidateToken?
        /// - Convenience method for common operation
        /// - Returns typed Guid instead of string claim value
        /// - Handles parsing and error cases
        /// 
        /// Used by:
        /// - Middleware that needs user ID
        /// - Services that need to identify current user
        /// - Audit logging
        /// </summary>
        /// <param name="token">JWT token string</param>
        /// <returns>User ID as Guid if valid, null if invalid/expired/not found</returns>
        Guid? GetUserIdFromToken(string token);
    }

}
