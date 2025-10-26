using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.DTOs.Auth
{
    /// <summary>
    /// SocialLoginRequest - Contains the OAuth code from provider
    /// Theory: OAuth 2.0 Authorization Code Flow
    /// 1. User clicks "Login with Google"
    /// 2. Redirected to Google's login page
    /// 3. User approves, Google redirects back with 'code'
    /// 4. We exchange 'code' for access token and user info
    /// </summary>
    public class SocialLoginRequest
    {
        /// <summary>
        /// Provider name: "Google", "GitHub", "Microsoft"
        /// </summary>
        [Required]
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Authorization code from OAuth provider
        /// Theory: This is NOT the access token, it's a one-time code
        /// We exchange this for the actual access token server-side
        /// Prevents exposing access token to client
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Redirect URI used in OAuth flow
        /// Theory: Must match exactly what was registered with provider
        /// Security measure to prevent authorization code interception
        /// </summary>
        [Required]
        public string RedirectUri { get; set; } = string.Empty;
    }

}
