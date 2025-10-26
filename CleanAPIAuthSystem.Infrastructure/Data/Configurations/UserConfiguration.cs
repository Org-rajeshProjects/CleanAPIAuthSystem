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
    /// <summary>
    /// User Entity Configuration - Fluent API configuration for User table
    /// Theory: IEntityTypeConfiguration<T> interface
    /// Separates configuration from DbContext for better organization
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name
            // Theory: By default, EF Core pluralizes (User -> Users)
            // Explicitly set for clarity and control
            builder.ToTable("Users");

            // Primary key
            // Theory: Explicitly define primary key (though EF Core infers from "Id")
            // Good practice for documentation
            builder.HasKey(u => u.Id);

            // Email configuration
            builder.Property(u => u.Email)
                .IsRequired() // NOT NULL constraint
                .HasMaxLength(256); // VARCHAR(256) - prevents unlimited text

            // Unique index on Email
            // Theory: Ensures no duplicate emails
            // Creates database index for fast lookups
            // Index name convention: IX_TableName_ColumnName
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            // UserName configuration
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            // Unique index on UserName
            builder.HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserName");

            // Password hash - nullable for social login users
            builder.Property(u => u.PasswordHash)
                .HasMaxLength(500); // Hashes are fixed length, but allow some buffer

            // Name fields
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // Boolean fields - default values
            // Theory: SQL Server BIT column, defaults to false
            builder.Property(u => u.IsEmailVerified)
                .HasDefaultValue(false);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            // Audit fields
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()"); // SQL Server function for current time

            // One-to-Many: User -> RefreshTokens
            // Theory: One user can have many refresh tokens
            // Foreign key: RefreshToken.UserId references User.Id
            // Cascade delete: When user is deleted, delete their tokens
            builder.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade); // CASCADE DELETE in database

            // One-to-Many: User -> SocialLogins
            builder.HasMany(u => u.SocialLogins)
                .WithOne(sl => sl.User)
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


}
