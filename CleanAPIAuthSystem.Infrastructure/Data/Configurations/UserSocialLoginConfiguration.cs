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
    public class UserSocialLoginConfiguration : IEntityTypeConfiguration<UserSocialLogin>
    {
        public void Configure(EntityTypeBuilder<UserSocialLogin> builder)
        {
            builder.ToTable("UserSocialLogins");

            builder.HasKey(sl => sl.Id);

            // Provider and ProviderKey
            builder.Property(sl => sl.Provider)
                .IsRequired()
                .HasMaxLength(50); // "Google", "GitHub", "Microsoft"

            builder.Property(sl => sl.ProviderKey)
                .IsRequired()
                .HasMaxLength(256); // Provider's user ID

            // Unique constraint: One provider account per user
            // Theory: User can't link same Google account twice
            // But can link Google + GitHub
            builder.HasIndex(sl => new { sl.UserId, sl.Provider, sl.ProviderKey })
                .IsUnique()
                .HasDatabaseName("IX_UserSocialLogins_User_Provider");

            // Index for finding user by social login
            // Theory: "Find user with Google ID 12345"
            builder.HasIndex(sl => new { sl.Provider, sl.ProviderKey })
                .HasDatabaseName("IX_UserSocialLogins_Provider_Key");

            // ProviderData - JSON field
            // Theory: SQL Server JSON column for flexible storage
            builder.Property(sl => sl.ProviderData)
                .HasColumnType("nvarchar(max)"); // Store as JSON text
        }
    }
}
