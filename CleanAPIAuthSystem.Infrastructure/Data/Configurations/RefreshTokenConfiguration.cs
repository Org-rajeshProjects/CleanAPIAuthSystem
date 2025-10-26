using CleanAPIAuthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            // Token string configuration
            // Theory: Tokens are ~44 characters (Base64 of 256 bits)
            // Allow 500 for future-proofing
            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);

            // Unique index on Token for fast lookups
            // Theory: When user sends refresh token, we need to find it quickly
            builder.HasIndex(rt => rt.Token)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_Token");

            // Composite index for user queries
            // Theory: Often query: "Get active tokens for user X"
            // Composite index (UserId, IsRevoked, ExpiresAt) optimizes this
            builder.HasIndex(rt => new { rt.UserId, rt.IsRevoked, rt.ExpiresAt })
                .HasDatabaseName("IX_RefreshTokens_User_Status");

            // IP Address fields
            builder.Property(rt => rt.CreatedByIp)
                .HasMaxLength(45); // IPv6 max length

            builder.Property(rt => rt.RevokedByIp)
                .HasMaxLength(45);

            // ReplacedByToken field
            builder.Property(rt => rt.ReplacedByToken)
                .HasMaxLength(500);

            // Foreign key relationship defined in UserConfiguration
            // No need to repeat here
        }
    }
}
