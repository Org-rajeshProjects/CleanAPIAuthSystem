using CleanAPIAuthSystem.Domain.Entities;
using CleanAPIAuthSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Infrastructure.Data
{
    /// <summary>
    /// ApplicationDbContext - EF Core Database Context
    /// Theory: DbContext represents a session with the database
    /// - Tracks entity changes
    /// - Translates LINQ queries to SQL
    /// - Manages database connections
    /// - Handles transactions
    /// 
    /// DbContext is the Unit of Work pattern in EF Core
    /// SaveChanges() commits all tracked changes in one transaction
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor with options
        /// Theory: Options pattern for configuration
        /// Database connection string, logging, etc. passed via options
        /// Configured in Startup/Program.cs
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet properties - Represent database tables
        /// Theory: DbSet<T> is a collection of entities
        /// LINQ queries on DbSet translate to SQL
        /// Example: Users.Where(u => u.Email == "test@test.com")
        /// Becomes: SELECT * FROM Users WHERE Email = 'test@test.com'
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<UserSocialLogin> UserSocialLogins { get; set; } = null!;

        /// <summary>
        /// Model configuration
        /// Theory: Fluent API for configuring entities
        /// Alternative to Data Annotations (more powerful)
        /// - Define relationships
        /// - Set constraints (unique, required, max length)
        /// - Configure indexes
        /// - Set table names
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from assembly
            // Theory: Convention - keep configurations in separate files
            // IEntityTypeConfiguration<T> interface for each entity
            // Keeps this file clean and organized
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply global query filters
            // Theory: Soft delete pattern - filter out deleted records automatically
            // Every query automatically adds: WHERE IsDeleted = false
            ApplyGlobalQueryFilters(modelBuilder);
        }

        /// <summary>
        /// SaveChanges override - Intercept before saving
        /// Theory: Cross-cutting concerns handled automatically
        /// - Set audit timestamps
        /// - Handle soft deletes
        /// - Enforce business rules
        /// Called internally by UnitOfWork.CompleteAsync()
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get all tracked entities
            // Theory: ChangeTracker keeps list of entities EF Core is tracking
            // Knows which are Added, Modified, Deleted
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                // Handle audit timestamps
                // Theory: IAuditableEntity interface marks entities that need auditing
                if (entry.Entity is IAuditableEntity auditableEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            // New entity - set creation timestamp
                            auditableEntity.CreatedAt = DateTime.UtcNow;
                            break;

                        case EntityState.Modified:
                            // Updated entity - set update timestamp
                            auditableEntity.UpdatedAt = DateTime.UtcNow;
                            break;
                    }
                }

                // Handle soft delete
                // Theory: Instead of DELETE, convert to UPDATE SET IsDeleted = true
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable softDeletable)
                {
                    // Change from Delete to Modified
                    entry.State = EntityState.Modified;
                    softDeletable.IsDeleted = true;
                    softDeletable.DeletedAt = DateTime.UtcNow;
                }
            }

            // Call base SaveChanges to actually persist to database
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Apply global query filters
        /// Theory: Automatically filter soft-deleted entities from all queries
        /// Without this, every query needs: .Where(x => !x.IsDeleted)
        /// </summary>
        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            // Get all entity types implementing ISoftDeletable
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    // Build lambda expression: entity => !entity.IsDeleted
                    // Theory: Expression trees allow building LINQ queries dynamically
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
                    var filter = Expression.Lambda(Expression.Not(property), parameter);

                    // Apply filter to entity
                    // Theory: HasQueryFilter adds WHERE clause to all queries
                    entityType.SetQueryFilter(filter);
                }
            }
        }
    }


}
