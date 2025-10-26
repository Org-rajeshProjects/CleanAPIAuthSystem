using CleanAPIAuthSystem.Application.Common;
using CleanAPIAuthSystem.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Interfaces.Services
{
    /// <summary>
    /// IAuthService - Authentication service interface
    /// </summary>
    public interface IAuthService
    {
        Task<Result<AuthResponse>> RegisterAsync(
            RegisterRequest request,
            string ipAddress,
            CancellationToken cancellationToken = default);

        Task<Result<AuthResponse>> LoginAsync(
            LoginRequest request,
            string ipAddress,
            CancellationToken cancellationToken = default);

        Task<Result<AuthResponse>> SocialLoginAsync(
            SocialLoginRequest request,
            string ipAddress,
            CancellationToken cancellationToken = default);

        Task<Result<AuthResponse>> RefreshTokenAsync(
            string refreshToken,
            string ipAddress,
            CancellationToken cancellationToken = default);

        Task<Result> RevokeTokenAsync(
            string refreshToken,
            string ipAddress,
            CancellationToken cancellationToken = default);

        Task<Result> RevokeAllUserTokensAsync(
            Guid userId,
            string ipAddress,
            CancellationToken cancellationToken = default);
    }

}
